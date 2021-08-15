class BlazorClientStorage {
    open(name, version, data) {
        console.log(`open`, name, version, data);
        if (this.idbDatabase)
            return Promise.resolve();

        return new Promise((resolve, reject) => {
            console.log(`window.indexedDB.open`, name, version);
            const dbOpenDBRequest = window.indexedDB.open(name, version);
            dbOpenDBRequest.onerror = (event) => reject(event);
            dbOpenDBRequest.onsuccess = () => {
                this.idbDatabase = dbOpenDBRequest.result;
                resolve(dbOpenDBRequest.result);
            };
            dbOpenDBRequest.onupgradeneeded = () => this.handleOnUpgradeNeeded(dbOpenDBRequest.result, data);
        });
    }

    handleOnUpgradeNeeded(idbDatabase, data) {
        const handleOnUpgradeNeededEvent = new CustomEvent('BlazorClientStorage.handleOnUpgradeNeeded', { detail: { idbDatabase, data } });
        window.dispatchEvent(handleOnUpgradeNeededEvent);

        console.log(`handleOnUpgradeNeeded`, idbDatabase, data);

        for (const storeData of data) {
            const store = idbDatabase.createObjectStore(storeData.name, storeData.options);
            if (storeData.indices) {
                for (const index of storeData.indices) {
                    store.createIndex(index.name, index.name, index.options);
                }
            }
        }
    }

    executeWithTransaction(storeName, action) {
        return new Promise((resolve, reject) => {
            console.log(`transaction`, storeName, "readwrite");

            const transaction = this.idbDatabase.transaction(storeName, "readwrite");
            transaction.onerror = e => { console.log(`transaction reject`, e); reject(e.target.error.message); }
            transaction.oncomplete = _ => { console.log(`transaction resolve`); resolve(); }

            const store = transaction.objectStore(storeName);
            action(store, transaction);
        });
    }

    add(storeName, model) {
        return new Promise((resolve, reject) => {
            console.log(`transaction`, storeName, "readwrite");
            const transaction = this.idbDatabase.transaction(storeName, "readwrite");
            const store = transaction.objectStore(storeName);

            delete model.key;
            const request = store.add(model);
            transaction.onerror = e => { console.log(`transaction reject`, e); reject(e.target.error.message); }
            transaction.oncomplete = _ => {
                console.log(`transaction resolve`, request.result);
                resolve(request.result);
            };
        });
    }

    put(storeName, model) {
        return this.executeWithTransaction(storeName, (store, transaction) => {
            console.log(`put`, model);
            store.put(model);
        });
    }

    get(storeName, key) {
        return new Promise((resolve, reject) => {
            console.log(`transaction`, storeName, "readwrite");
            const transaction = this.idbDatabase.transaction(storeName, "readonly");
            const store = transaction.objectStore(storeName);

            console.log(`get`, key);
            var request = store.get(key);
            request.onerror = (e) => { console.log(`getAll reject`, e); reject(e.target.error.message); }
            request.onsuccess = (e) => resolve(e.target.result);
        });
    }

    delete(storeName, key) {
        return this.executeWithTransaction(storeName, (store, transaction) => {
            console.log(`delete`, key);
            store.delete(key);
        });
    }

    getAll(storeName) {
        return new Promise((resolve, reject) => {
            const transaction = this.idbDatabase.transaction(storeName, "readonly");
            const store = transaction.objectStore(storeName);
            var request = store.getAll();
            request.onerror = (e) => { console.log(`getAll reject`, e); reject(e.target.error.message); }
            request.onsuccess = (e) => resolve(e.target.result);
        });
    }

    openCursor(storeName) {
        return new Promise((resolve, reject) => {
            const transaction = this.idbDatabase.transaction(storeName, "readonly");
            const store = transaction.objectStore(storeName);

            var results = [];
            const cursor = store.openCursor();
            cursor.onerror = (e) => { console.log(`cursor reject`, e); reject(e.target.error.message); }
            cursor.onsuccess = function (event) {
                var cursor = event.target.result;
                if (cursor) {
                    const model = cursor.value;
                    model.key = cursor.key;
                    results.push(model);
                    cursor.continue();
                } else {
                    console.log(results);
                    resolve(results);
                }
            };
        });
    }
}

window.BlazorClientStorage = new BlazorClientStorage();