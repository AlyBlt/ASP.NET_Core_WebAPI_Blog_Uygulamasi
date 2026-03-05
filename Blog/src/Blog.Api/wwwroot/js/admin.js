const CAT_API = "https://localhost:7281/api/categories";
const TAG_API = "https://localhost:7281/api/tags";
const token = localStorage.getItem("token");

document.addEventListener("DOMContentLoaded", () => {
    const role = localStorage.getItem("role");
    if (!(role == "1" || role?.toLowerCase() === "admin")) {
        alert("Unauthorized access!");
        window.location.href = "index.html";
        return;
    }
    loadCategories();
    loadTags();
});

// --- CATEGORIES ---
async function loadCategories() {
    const res = await fetch(CAT_API, { headers: { "Authorization": "Bearer " + token } });
    const data = await res.json();
    const list = document.getElementById("categoryList");
    list.innerHTML = data.map(c => `
        <li class="list-group-item d-flex justify-content-between align-items-center">
            <span>${c.name}</span>
            <div>
                <button class="btn btn-sm btn-outline-warning me-1" onclick="openUpdateModal(${c.id}, '${c.name}', 'category')">Edit</button>
                <button class="btn btn-sm btn-outline-danger" onclick="deleteItem(${c.id}, 'category')">Delete</button>
            </div>
        </li>
    `).join('');
}

// --- TAGS ---
async function loadTags() {
    const res = await fetch(TAG_API, { headers: { "Authorization": "Bearer " + token } });
    const data = await res.json();
    const list = document.getElementById("tagList");
    list.innerHTML = data.map(t => `
        <li class="list-group-item d-flex justify-content-between align-items-center">
            <span>#${t.name}</span>
            <div>
                <button class="btn btn-sm btn-outline-warning me-1" onclick="openUpdateModal(${t.id}, '${t.name}', 'tag')">Edit</button>
                <button class="btn btn-sm btn-outline-danger" onclick="deleteItem(${t.id}, 'tag')">Delete</button>
            </div>
        </li>
    `).join('');
}

// --- MODAL AÇMA ---
function openUpdateModal(id, name, type) {
    document.getElementById("updateId").value = id;
    document.getElementById("updateName").value = name;
    document.getElementById("updateType").value = type;

    // Modal'ı tetikle
    const modalElement = document.getElementById('updateModal');
    const modalInstance = new bootstrap.Modal(modalElement);
    modalInstance.show();
}

// --- GÜNCELLEME ONAY (Kategori & Tag Ayrımı) ---
async function confirmUpdate() {
    const id = document.getElementById("updateId").value;
    const name = document.getElementById("updateName").value.trim();
    const type = document.getElementById("updateType").value;
    const api = type === 'category' ? CAT_API : TAG_API;

    if (!name) return;

    // Güncelleme (PUT) yaparken Id göndermek ZORUNLUDUR.
    const updateData = {
        id: parseInt(id),
        name: name
    };

    // Sadece kategorilerde slug alanı var
    if (type === 'category') {
        updateData.slug = name.toLowerCase()
            .replace(/ /g, "-")
            .replace(/[^\w-]+/g, "");
    }

    const res = await fetch(`${api}/${id}`, {
        method: "PUT",
        headers: {
            "Content-Type": "application/json",
            "Authorization": "Bearer " + token
        },
        body: JSON.stringify(updateData)
    });

    if (res.ok) {
        const modalElement = document.getElementById('updateModal');
        const modalInstance = bootstrap.Modal.getInstance(modalElement);
        modalInstance.hide();
        type === 'category' ? loadCategories() : loadTags();
    } else {
        alert("Güncelleme sırasında sunucu hatası oluştu.");
    }
}

// --- SİLME ---
async function deleteItem(id, type) {
    const confirmMessage = type === 'category'
        ? "Are you sure you want to delete this category?"
        : "Are you sure you want to delete this tag?";

    if (!confirm(confirmMessage)) return;

    const api = type === 'category' ? CAT_API : TAG_API;

    try {
        const res = await fetch(`${api}/${id}`, {
            method: "DELETE",
            headers: { "Authorization": "Bearer " + token }
        });

        if (res.ok) {
            alert(`${type === 'category' ? 'Category' : 'Tag'} deleted successfully.`);
            type === 'category' ? loadCategories() : loadTags();
        } else {
            // Check if the error is due to database restriction
            if (type === 'category') {
                alert("❌ DELETE RESTRICTED: This category contains articles. Please move or delete the articles before deleting the category.");
            } else {
                alert("❌ Error: This item cannot be deleted at the moment.");
            }
        }
    } catch (error) {
        console.error("Delete error:", error);
        alert("A connection error occurred.");
    }
}

// --- EKLEME FONKSİYONLARI (Kategori/Tag) ---
async function addCategory() {
    const inputElement = document.getElementById("newCatName");
    const name = inputElement.value.trim();

    if (!name) return;

    // PascalCase kullanarak Backend DTO'su ile tam uyum sağlıyoruz
    const categoryDto = {
        Name: name,
        Slug: name.toLowerCase()
            .replace(/ /g, "-")
            .replace(/[^\w-]+/g, "")
    };

    const res = await fetch(CAT_API, {
        method: "POST",
        headers: {
            "Content-Type": "application/json",
            "Authorization": "Bearer " + token
        },
        body: JSON.stringify(categoryDto)
    });

    if (res.ok) {
        inputElement.value = "";
        loadCategories();
    } else {
        const errorData = await res.json().catch(() => null);
        alert("Create failed: " + (errorData?.message || "Check fields"));
    }
}

async function addTag() {
    const name = document.getElementById("newTagName").value.trim();
    if (!name) return;
    const res = await fetch(TAG_API, {
        method: "POST",
        headers: { "Content-Type": "application/json", "Authorization": "Bearer " + token },
        body: JSON.stringify({ name: name })
    });
    if (res.ok) { document.getElementById("newTagName").value = ""; loadTags(); }
}