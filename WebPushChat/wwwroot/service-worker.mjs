console.log("Service Worker started");

self.addEventListener("install", e => {
    console.log("Service Worker installed", e);
});

self.addEventListener("push", e => {
    e.waitUntil((async () => {
        console.log("Push message received", e.data.text());

        const data = e.data.json();
        if (!(typeof data === "object" && "sender" in data && "message" in data)) {
            console.log("Invalid message from push:", e);
            return;
        }

        let focusedWindow = false;
        const windows = await self.clients.matchAll({ type: "window", includeUncontrolled: true });
        for (const window of windows) {
            window.postMessage({ sender: data.sender, message: data.message });
            focusedWindow |= window.focused;
        }
        if (!focusedWindow) {
            await self.registration.showNotification(`${data.sender} says`, {
                body: data.message,
            });
        }
    })());
});
