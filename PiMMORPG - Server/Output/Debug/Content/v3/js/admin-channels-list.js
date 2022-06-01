$(document).ready(function()
{
	$(document).on("click", "#open-channel", function (e)
	{
		e.preventDefault();
		var id = $(this).attr("ref-id");
		var label = $("#list-channels");

		var ret = API.openChannel(id);
		switch(ret)
		{
			case 0:
			label.success("Channel opened successfuly!");

			$(this).attr("id", "close-channel");
			$(this).text("Close");
			break;
			case 1:
			label.warning("This channel is already opened! Please refresh the page...");
			break;
			case 2:
			label.error("Failed to open the channel!");
			break
			case -1:
			label.warning("This channel cannot be found! Please refresh the page...");
			break;
			default:
			label.error("Invalid return (" + ret + ")!");
			break;
		}
		return false;

	}).on("click", "#close-channel", function (e)
	{
		e.preventDefault();
		var id = $(this).attr("ref-id");
		var label = $("#list-channels");

		var ret = API.closeChannel(id);
		switch(ret)
		{
			case 0:
			label.success("Channel closed successfuly!");

			$(this).attr("id", "open-channel");
			$(this).text("Open");
			break;
			case 1:
			label.warning("This channel is not opened! Please refresh the page...");
			break;
			case 2:
			label.error("Failed to close the channel!");
			break
			case -1:
			label.warning("This channel cannot be found! Please refresh the page...");
			break;
			default:
			label.error("Invalid return (" + ret + ")!");
			break;
		}
		return false;
	}).on("click", "#load-channel", function (e) { return false; });

	function applyButton(tr, item)
	{
		var children = null;
		if((children = tr.find("#open-channel")).length == 0 && (children = tr.find("#close-channel")).length == 0)
			tr.append(children = $("<td><a class=\"btn btn-primary btn-block\" href=\"\" id=\"load-channel\" ref-id=\" " + item.Port + "\">Loading</a></td>"));
		children = children.children();

		var status = !API.channelStatus(item.Port);
		children.attr("id", status ? "open-channel" : "close-channel");
		children.text(status ? "Open"  : "Close");
	}

	function reloadChannels()
	{
		var container = $("#channels-table");
		var channels = API.listChannels();

		container.empty();

		if(channels != null)
		{
			$("#channels-update").text("Updated yesterday at " + new Date().toLocaleString("en-US"));
			function typeName(n)
			{
				switch(n)
				{
					case 0:
					return "RPG";
					case 1:
					return "BattleRoyale";
				}
			}

			$.each(channels, function(i, item)
			{
				var tr = $("<tr/>")
				.append($("<td/>").text(item.Name))
				.append($("<td/>").text(typeName(item.Type)))
				.append($("<td/>").text(item.IsPVP ? "Yes" : "No"))
				.append($("<td/>").text(item.Port))
				.append($("<td/>").text(item.MaximumConnections));

				applyButton(tr, item);
				container.append(tr);
			});
		}
	}
	reloadChannels();
});