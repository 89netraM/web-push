const messageForm = document.getElementById("message-form");
const messageBox = document.getElementById("message-box");
const messageList = document.getElementById("message-list");

let subscription = null;

messageForm.addEventListener("submit", e => {
    e.preventDefault();

    const sender = "You";
    const message = messageBox.value;
    messageBox.value = "";

    addMessage(new Date(), sender, message);

    if (sender != null) {
        fetch("./sendMessage", {
            method: "POST",
            headers: {
                "Content-Type": "application/json",
            },
            body: JSON.stringify({
                sender,
                message,
                subscription: subscription.toJSON()
            }),
        });
    }
}, false);

function addMessage(time, sender, message) {
    const item = document.createElement("li");
    item.setAttribute("data-time", time.toISOString());
    const senderElement = document.createElement("strong");
    senderElement.append(document.createTextNode(sender));
    item.append(senderElement);
    item.append(document.createTextNode(": "));
    const messageElement = document.createElement("span");
    messageElement.append(document.createTextNode(message));
    item.append(messageElement);
    messageList.prepend(item);
}

navigator.serviceWorker.addEventListener("message", e => {
    if (!(typeof e.data === "object" && "sender" in e.data && "message" in e.data)) {
        console.log("Invalid message from service worker:", e);
        return;
    }

    addMessage(new Date(), e.data.sender, e.data.message);
});

let registration = await navigator.serviceWorker.getRegistration("./service-worker.mjs");

if (registration == null) {
    registration = await navigator.serviceWorker.register("./service-worker.mjs", { scope: "./", type: "module" });
}

subscription = await registration.pushManager.getSubscription();
if (subscription == null) {
    const vapidPublicKey = await fetch("./vapidPublicKey").then(r => r.text());
    subscription = await registration.pushManager.subscribe({
        userVisibleOnly: true,
        applicationServerKey: vapidPublicKey,
    });
}
