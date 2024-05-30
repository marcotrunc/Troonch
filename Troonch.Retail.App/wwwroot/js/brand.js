let brandIdSelected = null;

const handleBrand = (checkbox = null) => {
    const buttonBox = document.getElementById('brand-action-buttons');
    const addBrandButton = document.getElementById('add-brand-button');
    const table = document.getElementById('brands-table');
    

    const checkboxes = table.querySelectorAll("input[type='checkbox']");
    

    checkboxes.forEach(item => {
        if (item !== checkbox) {
            item.checked = false;
        }
    });


    if (checkbox == null) {
        buttonBox.classList.add("d-none");
        addBrandButton.classList.remove("d-none");
        brandIdSelected = null;
        return;
    }

    if (checkbox.checked) {
        buttonBox.classList.remove("d-none");
        addBrandButton.classList.add("d-none");
        brandIdSelected = checkbox.value;
    } else {
        buttonBox.classList.add("d-none");
        addBrandButton.classList.remove("d-none");
        brandIdSelected = null;
    }
}

const showBrandModal = async () => {
    await renderHTML(`GetBrandForm\\${brandIdSelected}`, 'brand-modal-body', 'brand-modal');
}