function API() 
{
}

API.isLogged = false;
function baseCount(target)
{
	var result = -1;
	if(API.auth())
	{
		$.apiAjax(
		{
			url: target + "/count",
			success: function(data)
			{
				result = data;
			}
		});
	};
	return result;
}

API.auth = function()
{
	var result = false;
	$.apiAjax(
	{
		url: "auth/check",
		data: { key: $.cookie("key") },
		success: function(data) 
		{
			result = data;
		}
	});

	if(!result)
	{
		$.apiAjax(
		{
			url: "auth",
			data: { key: "1304ea97-c42d-4af4-9eb6-c17fb928f1d0" },
			success: function(data)
			{
				result = data.Result;
				if(result)
				{
					$.cookie("key", data.Key);
					result = true;
				}
			}
		});
	}

	return result;
};

API.login = function(username, password, remember)
{
	var result = -1;
	if(API.auth())
	{
		$.apiAjax(
		{
			url: "login",
			data:
			{
				username: username,
				password: password,
				remember: remember
			},
			success: function(data)
			{
				if(data.Result == 0)
				{
					this.isLogged = true;		
				}
				result = data.Result;
			}
		});
	};
	return result;
};

API.logout = function()
{
	if(API.auth())
	{
		$.apiAjax(
		{
			url: "logout",
			success: function(data)
			{
				var result = data.Result;
				if(result)
				{
					API.auth() = false;
					window.location.reload();
				}
				else
					alert("Falha ao realizar o logout!");
			}
		});
	}
	else
		alert("Falha na verificação de autenticação!");
};

API.getLogs = function()
{
	var result = null;
	if(API.auth())
	{
		$.apiAjax(
		{
			url: "logs",
			success: function(data)
			{
				result = data;
			}
		});
	}
	return result;
}

API.countAccounts = function() { return baseCount("accounts"); }
API.listAccounts = function()
{
	var result = null;
	if(API.auth())
	{
		$.apiAjax(
		{
			url: "accounts",
			success: function(data)
			{
				result = data;
			}
		});
	}
	return result;
}
API.registerAccount = function(username, password, rpassword, nickname)
{
	var result = -1;
	if(API.auth())
	{
		$.apiAjax(
		{
			url: "accounts/register",
			data:
			{
				username: username,
				password: password, 
				'repeat-password': rpassword,
				nickname: nickname
			},
			success: function(data)
			{
				result = data;
			}
		});
	}
	return result;
}

API.countChannels = function() { return baseCount("channels"); }
API.listChannels = function()
{
	var result = null;
	if(API.auth())
	{
		$.apiAjax(
		{
			url: "channels",
			success: function(data)
			{
				result = data;
			}
		});
	}
	return result;
}
API.createChannel = function(name, port, maxConns, type)
{
	var result = -1;
	if(API.auth())
	{
		$.apiAjax(
		{
			url: "channels/create",
			data:
			{
				name: name,
				port: port, 
				MaximumConnections: maxConns,
				type: type
			},
			success: function(data)
			{
				result = data;
			}
		});
	}
	return result;
}
API.openChannel = function(id)
{
	var result = -1;
	if(API.auth())
	{
		$.apiAjax(
		{
			url: "channels/open",
			data:
			{
				id: id,
			},
			success: function(data)
			{
				result = data;
			}
		});
	}
	return result;
}
API.closeChannel = function(id)
{
	var result = -1;
	if(API.auth())
	{
		$.apiAjax(
		{
			url: "channels/close",
			data:
			{
				id: id,
			},
			success: function(data)
			{
				result = data;
			}
		});
	}
	return result;
}
API.channelStatus = function(id)
{
	var result = -1;
	if(API.auth())
	{
		$.apiAjax(
		{
			url: "channels/status",
			data:
			{
				id: id,
			},
			success: function(data)
			{
				result = data;
			}
		});
	}
	//alert(result);
	return result;
}


API.countCharacters = function() { return baseCount("characters"); }
API.countAPIAccess = function() { return baseCount("api-access"); }