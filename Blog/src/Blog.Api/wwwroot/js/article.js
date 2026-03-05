const API = "https://localhost:7281/api/articles";
const token = localStorage.getItem("token");
const role = localStorage.getItem("role");
const userId = localStorage.getItem("userId");

// Sayfa yüklendiğinde çalışacak başlangıç kontrolleri
document.addEventListener("DOMContentLoaded", () => {
    if (!token) {
        alert("Please login first");
        window.location.href = "login.html";
        return;
    }

    // Role göre Create panelini gizle/göster
    if (role === "User") {
        const createSection = document.getElementById("createSection");
        if (createSection) createSection.style.display = "none";
    } else {
        // Sadece Admin veya Author ise kategorileri/tagleri yükle
        loadCategoriesAndTags();
    }

    // İlk makaleleri getir
    loadArticles();
});

// Kategorileri ve Tagleri Yükle
async function loadCategoriesAndTags() {
    try {
        const headers = { "Authorization": "Bearer " + token };

        // Kategoriler
        const resCat = await fetch('https://localhost:7281/api/categories', { headers });
        const categories = await resCat.json();
        const catSelect = document.getElementById("category");
        catSelect.innerHTML = categories.map(c => `<option value="${c.id}">${c.name}</option>`).join('');

        // Tagler
        const resTag = await fetch('https://localhost:7281/api/tags', { headers });
        const tags = await resTag.json();
        const tagsSelect = document.getElementById("tags");
        tagsSelect.innerHTML = tags.map(t => `<option value="${t.id}">${t.name}</option>`).join('');
    } catch (error) {
        console.error('Error loading metadata:', error);
    }
}

// Makaleleri Listele
async function loadArticles() {
    const response = await fetch(API, {
        method: "GET",
        headers: { "Authorization": "Bearer " + token }
    });

    if (!response.ok) {
        alert("Failed to load articles.");
        return;
    }

    const articles = await response.json();
    const list = document.getElementById("articles");
    list.innerHTML = "";

    articles.forEach(a => {
        const li = document.createElement("li");
        li.className = "list-group-item d-flex justify-content-between align-items-center";

        const info = document.createElement("div");
        info.innerHTML = `
            <a href="article.html?id=${a.id}" class="fw-bold text-decoration-none text-dark">${a.title}</a>
            <br><small class="text-muted">by ${a.authorName}</small>
        `;
        li.appendChild(info);

        // --- GÜNCEL YETKİ KONTROLÜ ---
        // Karşılaştırmayı garantiye almak için hem backend hem local verisini String'e çeviriyoruz
        const isAdmin = role === "Admin";
        const isOwner = a.authorId && userId && String(a.authorId) === String(userId);

        if (isAdmin || isOwner) {
            const actions = document.createElement("div");

            const updateBtn = document.createElement("button");
            updateBtn.className = "btn btn-warning btn-sm me-2";
            updateBtn.innerText = "Update";
            updateBtn.onclick = () => updateArticle(a.id);

            const deleteBtn = document.createElement("button");
            deleteBtn.className = "btn btn-danger btn-sm";
            deleteBtn.innerText = "Delete";
            deleteBtn.onclick = () => deleteArticle(a.id);

            actions.appendChild(updateBtn);
            actions.appendChild(deleteBtn);
            li.appendChild(actions);
        }

        list.appendChild(li);
    });
}

// Create Formunu Göster
function showCreateForm() {
    document.getElementById("createForm").style.display = "block";
    document.getElementById("showCreateBtn").style.display = "none";
}

// Makale Oluştur
async function createArticle() {
    const title = document.getElementById("title").value.trim();
    const content = document.getElementById("content").value.trim();
    const categoryId = document.getElementById("category").value;
    const tagsSelect = document.getElementById("tags");
    const tagIds = Array.from(tagsSelect.selectedOptions).map(option => option.value);

    if (!title || !content || !categoryId) {
        alert("Title, Content, and Category are required.");
        return;
    }

    const articleDto = { title, content, categoryId, tagIds, status: "Draft" };

    const response = await fetch(API, {
        method: "POST",
        headers: {
            "Content-Type": "application/json",
            "Authorization": "Bearer " + token
        },
        body: JSON.stringify(articleDto)
    });

    if (response.ok) {
        alert("Article created successfully!");
        loadArticles();
        document.getElementById("createForm").style.display = "none";
        document.getElementById("showCreateBtn").style.display = "block";
        document.getElementById("title").value = "";
        document.getElementById("content").value = "";
    } else {
        alert("Create failed.");
    }
}

// Güncelleme ve Silme Fonksiyonları
function updateArticle(id) {
    window.location.href = `updateArticle.html?id=${id}`;
}

async function deleteArticle(id) {
    if (!confirm("Do you want to delete this article?")) return;
    const response = await fetch(`${API}/${id}`, {
        method: "DELETE",
        headers: { "Authorization": "Bearer " + token }
    });

    if (response.ok) {
        alert("Deleted successfully.");
        loadArticles();
    } else {
        alert("Delete failed.");
    }
}

function logout() {
    localStorage.clear();
    window.location.href = "login.html";
}