const axios = require('axios');

async function checkLicenseKeyInAPI(key) {
    try {
        const response = await axios.post('https://api.ecrpc.online/validate', {
            license_key: key
        });

        return response.status === 200 && response.data.valid;
    } catch (error) {
        if (error.response) {
            console.error('Error validating license key:', error.message);
        } else if (error.request) {
            console.error('The API may be down or undergoing maintenance. Please contact the Vox Development Administration for more info.');
            process.exit(1);
        } else {
            console.error('Error:', error.message);
            process.exit(1);
        }
        return false;
    }
}

(async () => {
    const isValidLicense = await checkLicenseKeyInAPI(config.licenseKey);

    if (isValidLicense) {
        console.log('License key successfully validated. Enjoy your product.');
    } else {
        console.log('Exiting startup due to invalid license key.');
        process.exit(1);
    }
})();
