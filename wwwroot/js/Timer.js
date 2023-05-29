// Imports date and time from cshtml homepage file
var importedTimeStr = document.getElementById('conferenceTimer').value;
// The date of conference
const countDownDate = new Date(importedTimeStr).getTime();

// Run myfunc every second 
var myfunc = setInterval(function () {

    // Countdown logic
    var now = new Date().getTime();
    var timeleft = countDownDate - now;

    // Calculating the days, hours, minutes and seconds left
    var days = Math.floor(timeleft / (1000 * 60 * 60 * 24));
    var hours = Math.floor((timeleft % (1000 * 60 * 60 * 24)) / (1000 * 60 * 60));
    var minutes = Math.floor((timeleft % (1000 * 60 * 60)) / (1000 * 60));
    var seconds = Math.floor((timeleft % (1000 * 60)) / 1000);

    // Result is output to the specific element
    document.getElementById("totalTime").innerHTML = days + ":" + hours + ":" + minutes + ":" + seconds

    // Code below is not needed anymore, kept incase needed for the future - CKC
    /*
    // Not needed since timer values are sent in one getElement() call
    document.getElementById("days").innerHTML = days + "days: "
    document.getElementById("hours").innerHTML = hours + "hours: "
    document.getElementById("mins").innerHTML = minutes + "minutes: "
    document.getElementById("secs").innerHTML = seconds + "seconds: "

    // Display the message when countdown is over
    // Not needed since this logic is handle on the homepage cshtml file
    if (timeleft < 0) {
        clearInterval(myfunc);
        document.getElementById("days").innerHTML = ""
        document.getElementById("hours").innerHTML = ""
        document.getElementById("mins").innerHTML = ""
        document.getElementById("secs").innerHTML = ""
        document.getElementById("end").innerHTML = "TIME UP!!";
    }
    */
}, 1000);