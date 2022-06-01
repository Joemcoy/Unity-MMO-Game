$(document).ready(function()
{
	$("#register-account-form").submit(function()
	{
		var username = $("#username").val();
		var password = $("#password").val();
		var rpassword = $("#repeat-password").val();
		var nickname = $("#nickname").val();
		var label = $("#register-label");

		var res = API.registerAccount(username, password, rpassword, nickname);
		switch(res)
		{
			case 0:
			label.success("Sucessfully registered!");
			break;
			case 1:
			label.warning("Username must be at least 5 characters long.");
			break;
			case 2:
			label.warning("The password must be at least 5 characters long.");
			break;
			case 3:
			label.warning("Nickname must have at least 5 characters.");
			break;
			case 4:
			label.warning("Passwords do not match.");
			break;
			case 5:
			label.warning("The username is already being used!");
			break;
			case 6:
			label.warning("The nickname is already being used!");
			break;
			default:
			label.error("Unknown result (" + res + ")!");
			break;
		}

		return false;
	}).keyup(function(event) 
	{
		if (event.keyCode == 13) 
		{
			$(this).submit();
		}
	});
});