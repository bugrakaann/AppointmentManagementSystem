function renderCalendar(eventsURL, actions) {
    const calendarEl = document.getElementById('calendar');
    const calendarDate = $("#calendarDate");
    const calendar = new FullCalendar.Calendar(calendarEl, {
        initialView: 'timeGridWeek',
        firstDay: 1,
        allDaySlot: false,
        timeZone: 'local',
        locale: 'tr',
        slotLabelFormat: {
            hour: '2-digit',
            minute: '2-digit',
            hour12: false
        },
        eventTimeFormat: {
            hour: '2-digit',
            minute: '2-digit',
            hour12: false
        },
        headerToolbar: {
            left: '',
            right: '',
            center: 'prev,next'
        },
        selectable: true,
        select: actions.select,
        selectAllow: actions.selectAllow,
        datesSet: (info) => {
            loadCalendarEvents(info.start, info.end);
            $("#calendarDate").val(formattedDate(info.start));
        }
    });
    calendar.render();
    if (calendarDate.length) {
        calendarDate.on('change', function () {
            const selectedDate = $(this).val();
            selectedDate && calendar.gotoDate(selectedDate);
        });
    }

    function loadCalendarEvents(startDate, endDate) {
        startDate = formattedDate(startDate);
        endDate = formattedDate(endDate);
        $.ajax({
            url: eventsURL,
            type: 'GET',
            dataType: 'JSON',
            data: {
                startDate: startDate,
                endDate: endDate
            },
            success: function (slots) {
                slots.forEach(slot => {
                    const event = {
                        id: slot.id,
                        start: slot.startTime,
                        end: slot.endTime,
                        editable: false
                    }
                    if (slot.status === 1) {
                        event.title = "UYGUN DEĞİL";
                        event.color = "gray";
                    }
                    else if (slot.status === 2) {
                        event.title = "DOLU";
                        event.color = "red";
                    } else {
                        return;
                    }

                    if (!calendar.getEventById(event.id)) {
                        calendar.addEvent(event);
                    }
                });
            },
            error: function (error) {
                alert("Bir hata oluştu!")
            }
        });
    }
}

function formattedDate(dateTime) {
    return moment(dateTime).format("YYYY-MM-DD")
}

function formattedTime(dateTime) {
    return moment(dateTime).format("YYYY-MM-DD HH:mm:ss")
}