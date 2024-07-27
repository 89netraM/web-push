self.addEventListener("push", e => {
    e.waitUntil((async () => {
        const data = e.data.json();

        let focusedWindow = false;
        const windows = await self.clients.matchAll({ type: "window", includeUncontrolled: true });
        for (const window of windows) {
            window.postMessage(data);
            focusedWindow |= window.focused;
        }
        if (!focusedWindow) {
            if ("Sender" in data && "Message" in data) {
                await self.registration.showNotification(`${data.Sender} says`, {
                    body: data.Message,
                });
            }
        }
    })());
});
