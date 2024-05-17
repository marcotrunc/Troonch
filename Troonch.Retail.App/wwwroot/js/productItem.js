let productItemIdSelected = null;


const showVariationModal = async (path) => {
    await renderHTML(path + `/${productItemIdSelected}`, 'variation-modal-body', 'variation-modal');
}

const submitProductForm = async (event) => {

    try {
        event.preventDefault();

        const formId = 'product-form';

        const productForm = document.getElementById(formId);


        const formData = new FormData(productForm);

        cleanFormFromErrorMessage();

        let payload = setPayloadFromFormData(formData);

        disableForm(formId);

        const jsonData = JSON.stringify(payload);

        const response = await fetch('@Url.Action("Update", "Products")', {
            method: 'PUT',
            headers: {
                'Content-Type': 'application/json; charset=utf-8'
            },
            body: jsonData
        })

        if (!response.ok) {
            return await handleRequestInError(response, formId);
        }

        enableForm(formId);
        showNotification(false);

        
    }
    catch (error) {
        handleExceptionInFormWithRedirect(error);
    }
}

const handleVariation = (checkbox = null) => {
    const buttonBox = document.getElementById('variation-products-buttons');
    const addVariationButton = document.getElementById('add-variation-button-product');
    const table = document.getElementById('variations-table');
    const list = document.getElementById('variations-list');

    const checkboxes = table.querySelectorAll("input[type='checkbox']");
    const lCheckBoxes = list.querySelectorAll("input[type='checkbox']");

    checkboxes.forEach(item => {
        if (item !== checkbox) {
            item.checked = false;
        }
    });

    lCheckBoxes.forEach(item => {
        if (item !== checkbox) {
            item.checked = false;
        }
    });

    if (checkbox == null) {
        buttonBox.classList.add("d-none");
        addVariationButton.classList.remove("d-none");
        productItemIdSelected = null;
        return;
    }

    if (checkbox.checked) {
        buttonBox.classList.remove("d-none");
        addVariationButton.classList.add("d-none");
        productItemIdSelected = checkbox.value;
    } else {
        buttonBox.classList.add("d-none");
        addVariationButton.classList.remove("d-none");
        productItemIdSelected = null;
    }


}

const generateBarcode = async () => {
    
    try {
        const response = await fetch('/items/generatebarcode');

        if (!response.ok) {
            return await handleRequestInError(response);
        }

        const result = await response.json();

        const barcodeInput = document.getElementById('barcode');

        if (!barcodeInput) {
            return;
        }

        if (result.data && result.data != '') {
            barcodeInput.value = result.data;
        }
    }
    catch (error) {
        handleExceptionInFormWithRedirect(error);
    }
}

const renderProductItems = async () => {
    try {
        let productId = document.getElementById('product-id').value ?? '';
        await renderHTML(`/items/index/${productId}`, 'product-item-list-container');
    }
    catch (error) {
        console.error(error)
    }
}

const submitProductItemsForm = async (event, url) => {
    
    try {
        event.preventDefault();

        const formId = 'product-item-form';
        const productItemsForm = document.getElementById(formId);

        const formData = new FormData(productItemsForm);

        cleanFormFromErrorMessage();

        let payload = setPayloadFromFormData(formData);

        disableForm(formId);

        const jsonData = JSON.stringify(payload);

        let method = url.includes("Update") ? 'PUT' : 'POST';

        const response = await fetch(url, {
            method,
            headers: { 'Content-Type': 'application/json; charset=utf-8' },
            body: jsonData
        })

        if (!response.ok) {
            return await handleRequestInError(response, formId);
        }

        enableForm(formId);
        showNotification(false);
        handleVariation();
        closeModal('variation-modal');
        await renderProductItems();
    }
    catch (error) {
        handleExceptionInFormWithRedirect(error);
    }
}

const deleteProductItem = async () => {
    try {

        if (!productItemIdSelected) {
            return;
        }

        const response = await fetch(`/items/delete/${productItemIdSelected}`)

        if (!response.ok) {
            return await handleRequestInError(response);
        }

        showNotification(false);

        await renderProductItems();

        handleVariation();

    } catch (error) {
        handleExceptionInFormWithRedirect(error);
    }
}

