self.addEventListener("message", e => {
    if (!(typeof e.data === "object" && "message" in e.data)) {
        console.log("Invalid message from window:", e);
        return;
    }

    e.source.postMessage({ sender: "Service Worker", message: `You said: ${e.data.message}` });
});

self.addEventListener("push", e => {
    e.waitUntil((async () => {
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
