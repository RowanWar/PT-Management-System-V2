function showNotification(message, isSuccess = true) {
    const notification = document.getElementById('notification');

    
    notification.textContent = message;
    notification.className = isSuccess ? "success show" : "error show";

    // Automatically hide the notification after 3 seconds
    setTimeout(() => {
        notification.className = "hidden";
    }, 2400);
};