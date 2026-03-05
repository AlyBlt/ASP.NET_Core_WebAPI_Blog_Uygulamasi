const API = "https://localhost:7281/api/user"; // UserController route

async function login() {
    const username = document.getElementById("username").value;
    const password = document.getElementById("password").value;

    const response = await fetch(API + "/login", {
        method: "POST",
        headers: {
            "Content-Type": "application/json"
        },
        body: JSON.stringify({ userName: username, password })
    });

    const data = await response.json();

    if (response.ok) {
        // Backend'den dönen data yapısına göre (data.user.id) saklıyoruz
        localStorage.setItem("token", data.token);
        localStorage.setItem("role", data.user.role);
        localStorage.setItem("userId", data.user.id); 

        document.getElementById("result").innerText = "Login successful";
        window.location.href = "articles.html";
    } else {
        document.getElementById("result").innerText = data.message || "Login failed";
    }
}

async function register() {
    const username = document.getElementById("username").value;
    const email = document.getElementById("email").value;
    const password = document.getElementById("password").value;

    const response = await fetch(API + "/register", {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify({ userName: username, email, password })
    });

    const data = await response.json();

    if (response.ok) {
        document.getElementById("result").innerText = "Register successful";
    } else {
        document.getElementById("result").innerText = data.message || "Register failed";
    }
}