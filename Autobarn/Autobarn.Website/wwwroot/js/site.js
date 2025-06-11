// Write your JavaScript code.

function connectToSignalR() {
	console.log("Connecting to SignalR...");
	const conn = new signalR.HubConnectionBuilder().withUrl("/hub").build();
	conn.on("DisplayNewCarNotification", displayNewCarPopup);
	conn.start().then(function () {
		console.log("SignalR has started.");
	}).catch(function (err) {
		console.log(err);
	});
}

function displayNewCarPopup(user, message) {
	const data = JSON.parse(message);

	console.log(user);
	console.log(data);
	var $target = $("div#signalr-notifications");
	var html = `<div>
		<h3>NEW CAR ALERT!</h3>
		<p>${data.Make} ${data.Model} (${data.Color}, ${data.Year})</p>
		<p>Price: ${data.Price} ${data.CurrencyCode}</p>
		<p><a href="/vehicles/details/${data.Registration}">more details...</a></p>
	</div>`;
	console.log(html);
	$div = $(html);
	$div.css("background-color", data.Color);
	$target.prepend($div);
	window.setTimeout(function () {
		$div.fadeOut(3000, function () { $div.remove() });
	}, 5000);

}

// {"Registration":"58801D58","Make":"ABARTH","Model":"695","Year":1991,"Color":"LavenderBlush","Price":20910,"CurrencyCode":"EUR"}


$(document).ready(connectToSignalR);
