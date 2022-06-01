$(document).ready(function()
{
	var container = $("#accounts-table");
	var accounts = API.listAccounts();

	container.empty();

	if(accounts != null)
	{
		$("#accounts-update").text("Updated yesterday at " + new Date().toLocaleString("en-US"));

		$.each(accounts, function(i, item)
		{
			container.append($("<tr/>")
				.append($("<td/>").text(item.Username))
				.append($("<td/>").text(item.Nickname))
				.append($("<td/>").text(item.Access.Name).css("font-weight", "bolder"))
				.append($("<td/>").text(item.IsBanned ? "Yes" : "No"))
				.append($("<td/>").text(item.Serial)));
		});
	}
});