let categoryIdSelected = null;

const handleCategory = (checkbox = null) => {
    const buttonBox = document.getElementById('category-action-buttons');
    const addCategoryButton = document.getElementById('add-category-button');
    const table = document.getElementById('category-table');
    

    const checkboxes = table.querySelectorAll("input[type='checkbox']");
    

    checkboxes.forEach(item => {
        if (item !== checkbox) {
            item.checked = false;
        }
    });


    if (checkbox == null) {
        buttonBox.classList.add("d-none");
        addCategoryButton.classList.remove("d-none");
        categoryIdSelected = null;
        return;
    }

    if (checkbox.checked) {
        buttonBox.classList.remove("d-none");
        addCategoryButton.classList.add("d-none");
        categoryIdSelected = checkbox.value;
    } else {
        buttonBox.classList.add("d-none");
        addCategoryButton.classList.remove("d-none");
        categoryIdSelected = null;
    }
}

const resetCategoryIndexPage = async () => {
    categoryIdSelected = null;
    const table = document.getElementById('category-table');
    const checkboxes = table.querySelectorAll("input[type='checkbox']");

    checkboxes.forEach(item => item.checked = false);

    handleCategory();
}

const showCategoryModal = async () => 
    await renderHTML(`Categories\\GetCategoryForm\\${categoryIdSelected}`, 'category-modal-body', 'category-modal');

const submitCategoryForm = async (event) => {
    try {
        
        event.preventDefault();

        const formId = 'category-form';

        const categoryForm = document.getElementById(formId);

        const formData = new FormData(categoryForm);

        cleanFormFromErrorMessage();

        let payload = setPayloadFromFormData(formData);

        disableForm(formId);

        const jsonData = JSON.stringify(payload);

        const response = await fetch('/Categories/Create', {
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
        setTimeout(() => window.location.href = '/Categories', 1000);
    }
    catch (error) {
        handleExceptionInFormWithRedirect(error);
    }
}

const submitCategoryFormInUpdated = async (event) => {
    try {
      
        event.preventDefault();

        const formId = 'category-form';

        const categoryForm = document.getElementById(formId);

        const formData = new FormData(categoryForm);

        cleanFormFromErrorMessage();

        let payload = setPayloadFromFormData(formData);

        disableForm(formId);

        const jsonData = JSON.stringify(payload);

        const response = await fetch('/Categories/Update', {
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
        setTimeout(() => window.location.href = '/Categories', 1000);
    }
    catch (error) {
        handleExceptionInFormWithRedirect(error);
    }
};


const deleteCategory = async () => {
    try {

        if (!categoryIdSelected) {
            return;
        }

        const response = await fetch(`/Categories/${categoryIdSelected}`, {
            method: 'DELETE'
        });

        if (!response.ok) {
            return await handleRequestInError(response);
        }

        showNotification(false);

        setTimeout(() => window.location.href = '/Categories', 1000);

    } catch (error) {
        handleExceptionInFormWithRedirect(error);
    }
}