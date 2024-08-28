function renderCalendar(eventsURL, actions) {
    const calendarEl = document.getElementById('calendar');
    const calendarDate = $("#calendarDate");
    const calendar = new FullCalendar.Calendar(calendarEl, {
        initialView: 'timeGridWeek',
        firstDay: 1,
        allDaySlot: false,
        timeZone: 'local',
        locale: 'tr',
        businessHours: {
            daysOfWeek: [1, 2, 3, 4, 5],
            startTime: '06:00:00',
            endTime: "20:00:00",
        },
        hiddenDays: [6, 0],
        slotMinTime: "06:00:00",
        slotMaxTime: "20:00:00",
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
                    if (!slot?.props?.isValid) return;
                    const event = {
                        id: slot.id,
                        start: slot.startTime,
                        end: slot.endTime,
                        editable: false,
                        title: slot.props.title,
                        color: slot.props.colorCode
                    };
                    if (slot.url) {
                        event.url = slot.url;
                    }
                    if (slot.customer) {
                        event.title += " - " + slot.customer.name;
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