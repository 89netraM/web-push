self.addEventListener("push", e => {
    e.waitUntil((async () => {
        const data = e.data.json();
        if (!(typeof data === "object" && "senderId" in data && "sender" in data && "message" in data)) {
            console.log("Invalid message from push:", e);
            return;
        }

        let focusedWindow = false;
        const windows = await self.clients.matchAll({ type: "window", includeUncontrolled: true });
        for (const window of windows) {
            window.postMessage({ senderId: data.senderId, sender: data.sender, message: data.message });
            focusedWindow |= window.focused;
        }
        if (!focusedWindow) {
            await self.registration.showNotification(`${data.sender} says`, {
                body: data.message,
            });
        }
    })());
});
