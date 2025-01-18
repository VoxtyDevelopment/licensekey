-- ik this is made in a weird way but it's wtv you can change if needed

vox = {}

local licenseKey = Config.licensekey

vox.checkLicenseKeyInAPI = function(key, callback)
    local url = "API_URL"
    local body = json.encode({ license_key = key })
  
    PerformHttpRequest(url, function (errorCode, responseText, responseHeaders)
        if errorCode == 200 then
            local response_data = json.decode(responseText)
            callback(response_data.valid)
        else
            print("Error validating license key: HTTP status code " .. tostring(errorCode))
            callback(false)
        end
    end, 'POST', body, { ['Content-Type'] = 'application/json' })
  end
  
  AddEventHandler('onResourceStart', function(resourceName)
    if resourceName ~= GetCurrentResourceName() then
        return
    end
  
    vox.checkLicenseKeyInAPI(Config.licensekey, function(isValidLicense)
        if isValidLicense then
            print("License key successfully validated.")
        else
            print("Exiting resource startup due to invalid license key.")
            os.exit(1)
        end
    end)
  end)
