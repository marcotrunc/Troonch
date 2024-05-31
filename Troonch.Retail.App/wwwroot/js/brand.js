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

const resetBrandIndexPage = async () => {
    brandIdSelected = null;
    const table = document.getElementById('brands-table');
    const checkboxes = table.querySelectorAll("input[type='checkbox']");

    checkboxes.forEach(item => item.checked = false);

    handleBrand();
}

const showBrandModal = async () => 
    await renderHTML(`GetBrandForm\\${brandIdSelected}`, 'brand-modal-body', 'brand-modal');

const submitBrandForm = async (event) => {
    try {
        event.preventDefault();

        const formId = 'brand-form';

        const brandForm = document.getElementById(formId);

        const formData = new FormData(brandForm);

        cleanFormFromErrorMessage();

        let payload = setPayloadFromFormData(formData);

        disableForm(formId);

        const jsonData = JSON.stringify(payload);

        const response = await fetch('/Brands/Create', {
            method: 'POST',
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
        setTimeout(() => window.location.href = '/Brands', 1000);
    }
    catch (error) {
        handleExceptionInFormWithRedirect(error);
    }
}

const submitBrandFormInUpdated = async (event) => {
    try {
        
        event.preventDefault();

        const formId = 'brand-form';

        const brandForm = document.getElementById(formId);

        const formData = new FormData(brandForm);

        cleanFormFromErrorMessage();

        let payload = setPayloadFromFormData(formData);

        disableForm(formId);

        const jsonData = JSON.stringify(payload);

        const response = await fetch('/Brands/Update', {
            method: 'PUT',
            headers: {
                'Content-Type': 'application/json; charset=utf-8'
            },
            body: jsonData
        });

        if (!response.ok) {
            return await handleRequestInError(response, formId);
        }

        enableForm(formId);
        showNotification(false);
        setTimeout(() => window.location.href = '/Brands', 1000);
    }
    catch (error) {
        handleExceptionInFormWithRedirect(error);
    }
};


const deleteBrand = async () => {
    try {

        if (!brandIdSelected) {
            return;
        }

        const response = await fetch(`/brands/delete/${brandIdSelected}`)

        if (!response.ok) {
            return await handleRequestInError(response);
        }

        showNotification(false);

        setTimeout(() => window.location.href = '/Brands', 1000);

    } catch (error) {
        handleExceptionInFormWithRedirect(error);
    }
}