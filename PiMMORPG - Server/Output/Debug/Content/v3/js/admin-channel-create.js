$(document).ready(function()
{
	$("#create-channel-form").submit(function()
	{
		var label = $("#create-label");

		var name = $("#name").val();
		var port = $("#port").val();
		var maxConns = $("#maxConns").val();
		var type = $("#type").val();

		var res = API.createChannel(name, port, maxConns, type);
		switch(res)
		{
			case 0:
			label.success("Sucessfully created!");
			break;
			case 1:
			label.warning("Name must be at least 3 characters long.");
			break;
			case 2:
			label.warning("The port must in between 0~65535.");
			break;
			case 3:
			label.warning("Max connections must between 0~64000.");
			break;
			case 3:
			label.warning("");
			break;
			case 3:
			label.warning("Max connections must between 0~64000.");
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