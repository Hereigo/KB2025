# Small JS tips.

### Prevent Memory Leaks:

1- Use let and const: Avoid global variables.

2- Clean up event listeners: Always remove listeners when they’re no longer needed.

3- Break references: Set unused objects or variables to null.

4- Avoid excessive closures: Be mindful of closures, especially in loops.

5- Profile regularly: Use browser tools to monitor memory usage.


```js
// To edit any site - type in dev.tools:

document.designMode="on"


// Filter only numbers from Mixed Array (except of zero)

const arr = [null, 3, 0, 6, 7, -8, "", false];

const truthyArr = arr.filter(Boolean);

console.log(truthyArr); // =>  [ 3, 6, 7, -8 ]

// lookup template

$(function () {
    $('.js-address-container').each(function () {
        var $this = $(this);
        this.addEventListener('select-address', (e) => {
            SelectAddress(e, $this)
        });
    });
 
    function SelectAddress(e, $this) {
        SetOrHideAddress('[id*="Address1"]', e.detail.address1, $this);
        SetOrHideAddress('[id*="Address2"]', e.detail.address2, $this);
        SetOrHideAddress('[id*="Address3"]', e.detail.address3, $this);
        SetOrHideAddress('[id*="Town"]', e.detail.town, $this);
        SetOrHideAddress('[id*="Country"]', e.detail.country, $this)
        SetOrHideAddress('[id*="PostCode"]', e.detail.postcode, $this)
        $this.find('[id*="Uprn"]').val(e.detail.uprn);
        $this.find('.address-display').removeClass("hidden");
    }
 
    function SetOrHideAddress(selector, value, $this) {
        if (value == "") {
            $this.find(selector).val("");
            $this.find(selector).hide()
        }
        else {
            $this.find(selector).val(value);
            $this.find(selector).show()
        }
    }
})
```

```html
<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>Radio Button Insert Example</title>
    <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
</head>
<body>
    <div id="radio-buttons-container">
    </div>

    <script>
        $(document).ready(function() {
            $.ajax({
                url: 'API-ENDPOINT',
                type: 'GET',
                dataType: 'json',
                success: function(data) {
                    if (Array.isArray(data)) {
                        data.forEach(function(item) {
                            const radioButton = `
                                <label>
                                    <input type="radio" name="radioGroup" value="${item.number}">
                                    ${item.name}
                                </label><br>
                            `;
                            $('#radio-buttons-container').append(radioButton);
                        });
                    } else {
                        console.log("Returned data is not an array.");
                    }
                },
                error: function(xhr, status, error) {
                    console.error("Error fetching data: ", error);
                }
            });
        });
    </script>

</body>
</html>

```

```js
function showSuccessUploadStatus() {
    displayWarnings(null, 'Uploaded', "success");
}

function displayWarnings(warningHeader, warningDescription, warningType) {

    var warnBlock = getWarningsBlock();

    if (warningType === "notice") {
        warnBlock.removeClass("alert-danger alert-success");
        warnBlock.addClass("alert-warning");
    } else if (warningType === "success") {
        warnBlock.removeClass("alert-danger alert-warning");
        warnBlock.addClass("alert-success");
    } else {
        warnBlock.removeClass("alert-success alert-warning");
        warnBlock.addClass("alert-danger");
    }

    if (warningHeader) {
        getWarningHeaderElement().show();
        getWarningHeaderElement().text(warningHeader);
    }
    getWarningDescriptionElement().text(warningDescription);
    warnBlock.show();
}

function getWarningsBlock() {
    return $('#photo-upload-warnings');
}

function hideWarnings() {
    getWarningHeaderElement().hide();
    getWarningsBlock().hide();
}

$(function () {
    $('.js-some-container').each(function () {
        this.addEventListener('select-address', (e) => {
            selectAddress(e);
        });
    });
 
    function selectAddress(e) {
        setOrHideAddress('[id*="Address1"]', e.detail.address1, this);
        setOrHideAddress('[id*="Address2"]', e.detail.address2, this);
        setOrHideAddress('[id*="Address3"]', e.detail.address3, this);
        setOrHideAddress('[id*="Town"]', e.detail.town, this);
        setOrHideAddress('[id*="Country"]', e.detail.country, this)
        setOrHideAddress('[id*="PostCode"]', e.detail.postcode, this)
        $(this).find('[id*="Uprn"]').val(e.detail.uprn);
    }

})
```