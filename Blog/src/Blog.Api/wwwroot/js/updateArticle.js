const API = "https://localhost:7281/api/articles";
const token = localStorage.getItem("token");

// URL'den ID'yi al (örneğin: updateArticle.html?id=5)
const urlParams = new URLSearchParams(window.location.search);
const articleId = urlParams.get('id');

if (!articleId || !token) {
    window.location.href = "articles.html";
}

// Sayfa yüklendiğinde mevcut verileri doldur
document.addEventListener("DOMContentLoaded", async () => {
    await loadCategoriesAndTags(); // Önce dropdownlar dolmalı
    await fetchArticleDetails();   // Sonra makale verisi gelmeli
});

async function loadCategoriesAndTags() {
    const headers = { "Authorization": "Bearer " + token };
    try {
        const [resC, resT] = await Promise.all([
            fetch('https://localhost:7281/api/categories', { headers }),
            fetch('https://localhost:7281/api/tags', { headers })
        ]);

        const categories = await resC.json();
        const tags = await resT.json();

        document.getElementById("editCategory").innerHTML = categories.map(c =>
            `<option value="${c.id}">${c.name}</option>`).join('');

        document.getElementById("editTags").innerHTML = tags.map(t =>
            `<option value="${t.id}">${t.name}</option>`).join('');
    } catch (err) { console.error("Error loading selects", err); }
}

async function fetchArticleDetails() {
    try {
        const response = await fetch(`${API}/${articleId}`, {
            headers: { "Authorization": "Bearer " + token }
        });

        if (!response.ok) throw new Error("Article not found");

        const a = await response.json();

        // Formu doldur
        document.getElementById("editTitle").value = a.title;
        document.getElementById("editContent").value = a.content;
        document.getElementById("editCategory").value = a.categoryId;

        // Mevcut tagleri seçili hale getir
        const tagsSelect = document.getElementById("editTags");
        if (a.tagIds) {
            Array.from(tagsSelect.options).forEach(opt => {
                if (a.tagIds.includes(parseInt(opt.value))) opt.selected = true;
            });
        }
    } catch (err) {
        alert("Error loading article data");
    }
}

async function saveUpdate() {
    const updatedData = {
        id: parseInt(articleId),
        title: document.getElementById("editTitle").value,
        content: document.getElementById("editContent").value,
        categoryId: parseInt(document.getElementById("editCategory").value),
        tagIds: Array.from(document.getElementById("editTags").selectedOptions).map(o => parseInt(o.value)),
        status: "Published" // veya eski statusu koruyabilirsin
    };

    const response = await fetch(`${API}/${articleId}`, {
        method: "PUT",
        headers: {
            "Content-Type": "application/json",
            "Authorization": "Bearer " + token
        },
        body: JSON.stringify(updatedData)
    });

    if (response.ok) {
        alert("Article updated successfully!");
        window.location.href = "articles.html";
    } else {
        const err = await response.json();
        alert("Update failed: " + (err.message || "Unknown error"));
    }
}