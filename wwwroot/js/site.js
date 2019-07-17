//validation stuff
jQuery.validator.addMethod("accept", function(value, element) {
    return this.optional( element ) || /^([a-zA-Z0-9_.+-])+\@(([a-zA-Z0-9-])+\.)+([a-zA-Z0-9]{2,4})+$/.test( value );
    }, 'Please enter a valid email address');
jQuery.validator.addMethod("future", function(value, element) { 
    return this.optional(element) || Date.parse(value) < new Date().getTime(); //use Date.now() if you can instead of getTime, it's nicer
    }, "Please enter only future dates");
    
    $(".reservation").validate({
        rules: {
            date: {
                 required: true
            },
           
            time: {
                required: true,
            },
            Guests: {
                digits: true,
                min: 1,
                max: 6,
                required: true
            },
            FirstName: {
                minlength: 1,
                required: true
            },
            LastName: {
                minlength: 1,
                required: true
            },
            PhoneNumber: {
                digits: true,
                required: true
            }
        },
        messages: {
            date: "Please enter a valid date",
            time: "Please enter a valid time",
            Guests: "Please enter a number of guests (between 1 and 6)",
            FirstName: "Please enter a first name",
            LastName: "Please enter a last name",
            PhoneNumber: "Please enter a phone number (with no symbols or spaces)"
        }
    });

    $(".holidays").validate({
        rules: {
            startdate: {
                 required: true
            },
            enddate: {
                required: true
            },
            Reason: {
                minlength:	2,
                required: true
            }
        },
        messages: {
            startdate: "Please enter a valid date",
            enddate: "Please enter a valid date",
            Reason: "Please enter a reason"
        }
    });

$('#date').on('change', function () { $(this).valid(); });

function ul(index) {
	console.log('click!' + index)

	var underlines = document.querySelectorAll(".underline");

	for (var i = 0; i < underlines.length; i++) {
		underlines[i].style.transform = 'translate3d(' + index * 100 + '%,0,0)';
	}
}