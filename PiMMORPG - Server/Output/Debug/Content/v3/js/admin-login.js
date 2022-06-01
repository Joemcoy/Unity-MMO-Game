$(document).ready(function()
{
	$("#login-form").submit(function()
	{
		var label = $("#login-label");
		if(API.auth())
		{
			var username = $("#username").val();
			var password = $("#password").val();
			var remember = $("#remember").is(":checked");

			var res = API.login(username, password, remember);
			switch(res)
			{
				case 0:
				label.success("Logged in successfully!");
				window.location.reload();
				break;
				case 1:
				label.warning("Your username must be at least 5 characters long!");
				break;
				case 2:
				label.warning("Password must be at least 5 characters long!");
				break;
				case 3:
				label.warning("User not found!");
				break;
				case 4:
				label.warning("Passwords do not match!");
				break;
				case -1:
				label.warning("Internal error!");
				break;
				default:
				label.error("Unknown result (" + res + ")!");
				break;
			}
		}
		else
		{
			label.error("API Authentication Failed!");
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