const submitProductFormInUpdate = async (event) => {

    try {
        event.preventDefault();

        const formId = 'product-form';

        const productForm = document.getElementById(formId);


        const formData = new FormData(productForm);

        cleanFormFromErrorMessage();

        let payload = setPayloadFromFormData(formData);

        disableForm(formId);

        const jsonData = JSON.stringify(payload);

        const response = await fetch('/Products/Update', {
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

