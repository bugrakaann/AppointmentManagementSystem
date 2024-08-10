/*
Author       : Dreamguys
Template Name: Pathivu - Bootstrap Template
Version      : 1.0
*/

(function($) {
    "use strict";
	
	// Stick Sidebar
	
	if ($(window).width() > 767) {
		if($('.theiaStickySidebar').length > 0) {
			$('.theiaStickySidebar').theiaStickySidebar({
			  // Settings
			  additionalMarginTop: 70
			});
		}
	}	
	
	// Sticky Menu
	
	$(window).scroll(function(){
		var scroll = $(window).scrollTop();
			if (scroll > 70) {
				$(".min-header").addClass("sticky");
			}
			else{
				$(".min-header").removeClass("sticky");  	
			}
	})
	  
	// Search Bar
	
	$(document).ready(function() {
		$(".fa-search").click(function() {
			$(".togglesearch").toggle();
			$(".top-search").focus();
		});
	});
	
	// Sidebar
	
	if($(window).width() <= 991){
	var Sidemenu = function() {
		this.$menuItem = $('.main-nav a');
	};
	
	function init() {
		var $this = Sidemenu;		$('.main-nav a').on('click', function(e) {
			if($(this).parent().hasClass('has-submenu')) {
				e.preventDefault();
			}
			if(!$(this).hasClass('submenu')) {
				$('ul', $(this).parents('ul:first')).slideUp(350);
				$('a', $(this).parents('ul:first')).removeClass('submenu');
				$(this).next('ul').slideDown(350);
				$(this).addClass('submenu');
			} else if($(this).hasClass('submenu')) {
				$(this).removeClass('submenu');
				$(this).next('ul').slideUp(350);
			}
		});
	}

	// Sidebar Initiate
	
	init();
	}
	
	// Textarea Text Count
	
	var maxLength = 100;
	$('#review_desc').on('keyup change', function () {
		var length = $(this).val().length;
		 length = maxLength-length;
		$('#chars').text(length);
	});
	
	// Select 2
	
	if($('.select').length > 0) {
		$('.select').select2({
			minimumResultsForSearch: -1,
			width: '100%'
		});
	}
	 
    // Tooltip

	var tooltipTriggerList = [].slice.call(document.querySelectorAll('[data-bs-toggle="tooltip"]')) 
	var tooltipList = tooltipTriggerList.map(function (tooltipTriggerEl) { return new bootstrap.Tooltip(tooltipTriggerEl) })

	// Date Time Picker
	
	if($('.datetimepicker').length > 0) {
		$('.datetimepicker').datetimepicker({
			format: 'DD/MM/YYYY',
			icons: {
				up: "fas fa-chevron-up",
				down: "fas fa-chevron-down",
				next: 'fas fa-chevron-right',
				previous: 'fas fa-chevron-left'
			}
		});
	}
	
	// Floating Label

	if($('.floating').length > 0 ){
		$('.floating').on('focus blur', function (e) {
		$(this).parents('.form-focus').toggleClass('focused', (e.type === 'focus' || this.value.length > 0));
		}).trigger('blur');
	}
	
	// Mobile menu sidebar overlay
	
	$('body').append('<div class="sidebar-overlay"></div>');
	$(document).on('click', '#mobile_btn', function() {
		$('main-wrapper').toggleClass('slide-nav');
		$('.sidebar-overlay').toggleClass('opened');
		$('html').addClass('menu-opened');
		return false;
	});
	
	$(document).on('click', '.sidebar-overlay', function() {
		$('html').removeClass('menu-opened');
		$(this).removeClass('opened');
		$('main-wrapper').removeClass('slide-nav');
	});
	
	$(document).on('click', '#menu_close', function() {
		$('html').removeClass('menu-opened');
		$('.sidebar-overlay').removeClass('opened');
		$('main-wrapper').removeClass('slide-nav');
	});
	
	
	// Add More Hours
	
    $(".hours-info").on('click','.trash', function () {
		$(this).closest('.hours-cont').remove();
		return false;
    });

    $(".add-hours").on('click', function () {
		
		var hourscontent = '<div class="row form-row hours-cont" id="hoursModal>' +
			'<div class="col-12 col-md-10">' +
				'<div class="row form-row">' +
					'<div class="col-12 col-md-6">' +
						'<div class="form-group">' +
							'<label>Start Time</label>' +
							'<input class="form-control" name="startTime" type="datetime-local">' +
							'<label>End Time</label>' +
							'<input class="form-control" name="endTime" type="datetime-local">'+
						'</div>' +
					'</div>' +
				'</div>' +
			'</div>' +
			'<div class="col-12 col-md-2"><label class="d-md-block d-sm-none d-none">&nbsp;</label><a href="#" class="btn btn-danger trash"><i class="far fa-trash-alt"></i></a></div>' +
		'</div>';
		
        $(".hours-info").append(hourscontent);
        return false;
    });



	// Content div min height set
	
	function resizeInnerDiv() {
		var height = $(window).height();	
		var header_height = $(".header").height();
		var footer_height = $(".footer").height();
		var setheight = height - header_height;
		var trueheight = setheight - footer_height;
		$(".content").css("min-height", trueheight);
	}
	
	if($('.content').length > 0 ){
		resizeInnerDiv();
	}

	$(window).resize(function(){
		if($('.content').length > 0 ){
			resizeInnerDiv();
		}
	});
	
	// Slick Slider
	
	if($('.college-slider').length > 0) {
	$('.college-slider').slick({
		infinite: true,
		slidesToShow: 3,
		slidesToScroll: 1,
		responsive: [
			{
				breakpoint: 992,
				settings: {
					slidesToShow: 3
				}
			},
			{
			  breakpoint: 800,
			  settings: {
					slidesToShow: 2
				}
			},
			{
				breakpoint: 500,
				settings: {
					slidesToShow: 1
				}
			}		  
			]
		});
	}
	
	if($('.instructor-slider').length > 0) {
	$('.instructor-slider').slick({
		infinite: true,
		focusOnSelect: true,
		centerMode: true,
		centerPadding: '0',
		slidesToShow: 3,
		slidesToScroll: 1,
		responsive: [
			{
				breakpoint: 992,
				settings: {
					slidesToShow: 2,
					centerMode: false
				}
			},
			{
				breakpoint: 670,
				settings: {
					slidesToShow: 1,
					centerMode: false,
				}
			}
			]
		});
	}
	
	
	if($('.indexone .review-slider').length > 0) {
		$('.indexone .review-slider').slick({
			infinite: true,
			slidesToShow: 2,
			speed: 500,
			autoplay:false,
			responsive: [
				{
					breakpoint: 992,
					settings: {
						slidesToShow: 1
					}
				}
			]
		});
	}
	
	// Date Range Picker
	
	if($('.bookingrange').length > 0) {
		var start = moment().subtract(6, 'days');
		var end = moment();

		function booking_range(start, end) {
			$('.bookingrange span').html(start.format('MMMM D, YYYY') + ' - ' + end.format('MMMM D, YYYY'));
		}

		$('.bookingrange').daterangepicker({
			startDate: start,
			endDate: end,
			ranges: {
				'Today': [moment(), moment()],
				'Yesterday': [moment().subtract(1, 'days'), moment().subtract(1, 'days')],
				'Last 7 Days': [moment().subtract(6, 'days'), moment()],
				'Last 30 Days': [moment().subtract(29, 'days'), moment()],
				'This Month': [moment().startOf('month'), moment().endOf('month')],
				'Last Month': [moment().subtract(1, 'month').startOf('month'), moment().subtract(1, 'month').endOf('month')]
			}
		}, booking_range);

		booking_range(start, end);
	}
	
	// Chat

	var chatAppTarget = $('.chat-window');
	(function() {
		if ($(window).width() > 991)
			chatAppTarget.removeClass('chat-slide');
		
		$(document).on("click",".chat-window .chat-users-list a.media",function () {
			if ($(window).width() <= 991) {
				chatAppTarget.addClass('chat-slide');
			}
			return false;
		});
		$(document).on("click","#back_user_list",function () {
			if ($(window).width() <= 991) {
				chatAppTarget.removeClass('chat-slide');
			}	
			return false;
		});
	})();
	
		
	// Preloader
	
	$(window).on('load', function () {
		if($('#loader').length > 0) {
			$('#loader').delay(350).fadeOut('slow');
			$('body').delay(350).css({ 'overflow': 'visible' });
		}
	});
		
	//Home 2 
	
	// Sticky Menu
	
	$(window).on('scroll',function(){
		if ($(this).scrollTop()>60){
			$('.indextwo .header').addClass('nav-fixed');
	  		$('body.indextwo').addClass('map-up');
		} else {
			$('.indextwo .header').removeClass('nav-fixed');
	  		$('body.indextwo').removeClass('map-up');
		}
	});
	
	
	// Slick Slider
	
	if($('.indextwo .provider-slider').length > 0) {
		$('.indextwo .provider-slider').slick({
			dots: false,
			autoplay:false,
			infinite: true,
			slidesToShow: 4,
			slidesToScroll: 1,
			responsive: [{
				breakpoint: 992,
					settings: {
						slidesToShow: 3
				  	}
			},
			{
				breakpoint: 800,
					settings: {
						slidesToShow: 3
				  	}
			},
			{
				breakpoint: 776,
					settings: {
						slidesToShow: 2
				  	}
			},
			{
				breakpoint: 567,
					settings: {
						slidesToShow: 1
					}
			}]
		});
	}

	if($('.indextwo .features-slider').length > 0) {
		$('.indextwo .features-slider').slick({
			dots: true,
			infinite: true,
			centerMode: true,
			slidesToShow: 3,
			speed: 500,
			variableWidth: true,
			arrows: false,
			autoplay:false,
			responsive: [{
				  breakpoint: 992,
				  settings: {
					slidesToShow: 1
				  }

			},
			{
                breakpoint: 800,
                settings: "unslick"
            }]
		});
	}
	
	//News slider
	
	if($('.indextwo .carousel').length > 0) {
		var slickopts = {
		  slidesToShow: 3,
		  slidesToScroll: 1,
		  rows: 1,
		  responsive: [
		    { breakpoint: 992,
		      settings: {
		        slidesToShow: 3
		      }
		    },
		    { breakpoint: 776,
		      settings: {
		        slidesToShow: 2,
		        rows: 1
		      }
		    },
		    { breakpoint: 567,
		      settings: {
		        slidesToShow: 1,
		        rows: 1
		      }
		    }]
		};
		$('.indextwo .carousel').slick(slickopts);
	}
	
	// Home 3
	
	// Sticky Menu
	
	$(window).scroll(function(){
		var scroll = $(window).scrollTop();
			if (scroll > 70) {
				$(".indexthree .header").addClass("stickyHeader");
			}
			else{
				$(".indexthree .header").removeClass("stickyHeader");  	
			}
	})
	
	
	// Slick Slider
	
	if($('.indexthree .featured-school-slider').length > 0) {
		$('.featured-school-slider').slick({
			dots: false,
			autoplay:false,
			infinite: true,
			variableWidth: true,
		});
	}

	if($('.indexthree .top-school-slider').length > 0) {
		$('.top-school-slider').slick({
			dots: false,
			autoplay:false,
			infinite: true,
			variableWidth: true,
		});
	}

	if($('.indexthree .top-teacher-slider').length > 0) {
		$('.top-teacher-slider').slick({
			dots: false,
			autoplay:false,
			infinite: true,
			variableWidth: true,
		});
	}
	
	// Home 4
	
	if($('.indexfour .services-slider').length > 0) {
		$('.indexfour .services-slider').slick({
			dots: false,
			autoplay:false,
			infinite: true,
			variableWidth: true,
		});
	}

	if($('.indexfour .counsellors-slider').length > 0) {
		$('.indexfour .counsellors-slider').slick({
			slidesToShow: 5,
			slidesToScroll: 1,
			autoplay: true,
			autoplaySpeed: 2000,
			responsive: [
				{
				  breakpoint: 1024,
				  settings: {
					dots: false,
					autoplay:false,
					infinite: true,
					variableWidth: true,
				  }
				},
			]
		});
	}
	
	// Sticky Menu
	
	$(window).scroll(function(){
		var scroll = $(window).scrollTop();
			if (scroll > 70) {
				$(".headerfour .header").addClass("sticky");
			}
			else{
				$(".headerfour .header").removeClass("sticky");  	
			}
	})
	
	//Home 5
	
	//Services Slider
	
	if($('.indexfive .owl-carousel.services-list').length > 0) {
	$('.indexfive .owl-carousel.services-list').owlCarousel({
		loop:true,
		margin:15,
		nav:true,
		navText : ["<i class='fa fa-caret-left'></i>","<i class='fa fa-caret-right'></i>"],
		navContainer: '.mynav',
		responsive:{
			0:{
				items:1
			},
			500:{
				items:2
			},
			768:{
				items:3
			},
			1000:{
				items:4
			},
			1300:{
				items:4
			}
		}
	})
	}
	
	// Theraphist Slider
	
	if($('.indexfive .owl-carousel.theraphist').length > 0) {
	$('.indexfive .owl-carousel.theraphist').owlCarousel({
		loop:true,
		margin:15,
		nav:true,
		navText : ["<i class='fa fa-caret-left'></i>","<i class='fa fa-caret-right'></i>"],
		navContainer: '.mynav2',
		responsive:{
			0:{
				items:1
			},
			
			500:{
				items:2
			},
			700:{
				items:3
			},
			1000:{
				items:5
			}
		}
	})
	}

	// Reviews Slider
	
	if($('.indexfive .owl-carousel.reviews').length > 0) {
	$('.indexfive .owl-carousel.reviews').owlCarousel({
		loop:true,
		margin:15,
		nav:true,
		navText : ["<i class='fa fa-caret-left'></i>","<i class='fa fa-caret-right'></i>"],
		navContainer: '.mynav3',
		responsive:{
			0:{
				items:1
			},
			768:{
				items:2
			},
			1000:{
				items:3
			}
		}
	})
	}

	// News Slider
	
	if($('.indexfive .owl-carousel.news').length > 0) {
	$('.indexfive .owl-carousel.news').owlCarousel({
		loop:true,
		margin:15,
		nav:true,
		navText : ["<i class='fa fa-caret-left'></i>","<i class='fa fa-caret-right'></i>"],
		navContainer: '.mynav4',
		responsive:{
			0:{
				items:1
			},
			
			500:{
				items:2
			},
			768:{
				items:3
			},
			1000:{
				items:3
			}
		}
	})
	}
	
	// Home 6
	
	 //Scroll Header
	
	$(window).on('scroll',function(){
		if ($(this).scrollTop()>150){
			$('.indexsix .header').addClass('nav-fixed');
			$('.indexsix .map-right').addClass('map-top');
		} else {
			$('.indexsix .header').removeClass('nav-fixed');
			$('.indexsix .map-right').removeClass('map-top');
		}
	});
	
	
	if($('.indexsix #event-slider').length > 0 ){
		$('.indexsix #event-slider').owlCarousel({
			items: 5,
	        margin: 30,
	        dots : false,
			nav: true,
			navText: [
				'<i class="fas fa-caret-left"></i>',
				'<i class="fas fa-caret-right"></i>'
			],
			loop: true,
			responsiveClass:true,
	        responsive: {
	          	0: {
	            	items: 1
	          	},
	          	768 : {
	            	items: 3
	          	},
	          	1170: {
	            	items: 4
	          	}
	        }
	    });
    }
	
	if($('.indexsix #speaker-slider').length > 0 ){
		$('.indexsix #speaker-slider').owlCarousel({
			items: 5,
	        margin: 30,
	        dots : false,
			nav: true,
			navText: [
				'<i class="fas fa-caret-left"></i>',
				'<i class="fas fa-caret-right"></i>'
			],
			loop: true,
			responsiveClass:true,
	        responsive: {
	          	0: {
	            	items: 1
	          	},
	          	768 : {
	            	items: 3
	          	},
	          	1170: {
	            	items: 4
	          	}
	        }
	    });
    }
	
	if($('.indexsix #testimonial-slider').length > 0 ){
		$('.indexsix #testimonial-slider').owlCarousel({
			items: 5,
	        margin: 30,
	        dots : false,
			nav: true,
			navText: [
				'<i class="fas fa-caret-left"></i>',
				'<i class="fas fa-caret-right"></i>'
			],
			loop: true,
			responsiveClass:true,
	        responsive: {
	          	0: {
	            	items: 1
	          	},
	          	768 : {
	            	items: 2
	          	},
	          	1170: {
	            	items: 3
	          	}
	        }
	    });
    }
	
	// Home 7
	
	$(document).ready(function(){
	  $(window).scroll(function(){
		var scroll = $(window).scrollTop();
		  if (scroll > 50) {
			$(".indexseven .header-trans").css("background" , "#134a56");
			$(".indexseven .header-trans").css("top" , "0");
		  }

		  else{
			  $(".indexseven .header-trans").css("background" , "transparent");  	
			  $(".indexseven .header-trans").css("top" , "auto");  	
		  }
	  })
	});
	
	// Testimonial Slider

	if($('.indexseven #customers-testimonials').length > 0) {
		$('.indexseven #customers-testimonials').owlCarousel({
			nav: true,
			items: 1,
			autoplay: true,
			loop: true,
			autoplayTimeout: 50000,
			navText: ["<i class='fas fa-arrow-left owl-nav-left'></i>", "<i class='fas fa-arrow-right owl-nav-right ms-3'></i>"],
		});
	}
	
	// Home 8
	
	$(document).ready(function(){
	  $(window).scroll(function(){
		var scroll = $(window).scrollTop();
		  if (scroll > 150) {
			$(".indexeight .header-nav").css("background" , "#2D3952");
		  }

		  else{
			  $(".indexeight .header-nav").css("background" , "transparent");  	
		  }
	  })
	});
	
	//Libraries Slider
     
	if($('.indexeight .owl-carousel.libraries-items').length > 0) { 
	$('.indexeight .owl-carousel.libraries-items').owlCarousel({
		loop:true,
		margin:15,
		dots: false,
		nav:true,
		navContainer: '.slide-nav-1',
		responsive:{
			0:{
				items:1
			},
			500:{
				items:2
			},
			768:{
				items:2
			},
			1000:{
				items:3
			},
			1300:{
				items:5
			}
		}
	})
	}
	
	//Books Slider

    if($('.indexeight .owl-carousel.books-items').length > 0) { 
	$('.indexeight .owl-carousel.books-items').owlCarousel({
		loop:true,
		margin:15,
		dots: false,
		nav:true,
		navContainer: '.slide-nav-2',
		responsive:{
			0:{
				items:1
			},
			500:{
				items:2
			},
			768:{
				items:2
			},
			1000:{
				items:3
			},
			1300:{
				items:5
			}
		}
	})
	}
	
	//Librarian Slider

    if($('.indexeight .owl-carousel.librarian-items').length > 0) { 
	$('.indexeight .owl-carousel.librarian-items').owlCarousel({
		loop:true,
		margin:15,
		dots: false,
		nav:true,
		navContainer: '.slide-nav-3',
		responsive:{
			0:{
				items:1
			},
			500:{
				items:2
			},
			768:{
				items:2
			},
			1000:{
				items:3
			},
			1300:{
				items:5
			}
		}
	})
	}
	
	//Course Slider

     if($('.indexeight  .owl-carousel.course-items').length > 0) { 
	$('.indexeight .owl-carousel.course-items').owlCarousel({
		loop:true,
		margin:15,
		dots: false,
		nav:true,
		navContainer: '.slide-nav-4',
		responsive:{
			0:{
				items:1
			},
			500:{
				items:1
			},
			768:{
				items:2
			},
			1000:{
				items:3
			},
			1300:{
				items:3
			}
		}
	})
	 }
	 
	//Reviews Slider

     if($('.indexeight  .owl-carousel.reviews-items').length > 0) { 
	$('.indexeight .owl-carousel.reviews-items').owlCarousel({
		loop:true,
		margin:15,
		dots: false,
		nav:true,
		navContainer: '.slide-nav-5',
		responsive:{
			0:{
				items:1
			},
			500:{
				items:1
			},
			768:{
				items:2
			},
			1000:{
				items:3
			},
			1300:{
				items:3
			}
		}
	}) 
	 }
	 
	// Home 9
	
	//Scroll Header
	
	$(window).scroll(function() {
	  var sticky = $('.indexnine .min-header'),
	      scroll = $(window).scrollTop();
	  if (scroll >= 100) {
	  	sticky.addClass('nav-sticky');
	  	$('body.indexnine').addClass('map-up');
	  }
	  else {
	  	sticky.removeClass('nav-sticky');
	  	$('body.indexnine').removeClass('map-up');
	  } 

	});
	
	
		// Slick Slider
	
	if($('.indexnine .course-slider').length > 0) {
		$('.indexnine .course-slider').slick({
			infinite: true,
			slidesToShow: 3,
			slidesToScroll: 1,
			responsive: [{
				breakpoint: 992,
					settings: {
						slidesToShow: 3
				  	}
			},
			{
				breakpoint: 776,
					settings: {
						slidesToShow: 2
				  	}
			},
			{
				breakpoint: 567,
					settings: {
						slidesToShow: 1
					}
			}]
		});
	}

	
	if($('.indexnine .stylist-slider').length > 0) {
		$('.indexnine .stylist-slider').slick({
			infinite: true,
			slidesToShow: 4,
			slidesToScroll: 1,
			responsive: [{
				breakpoint: 992,
					settings: {
						slidesToShow: 3
				  	}
			},
			{
				breakpoint: 776,
					settings: {
						slidesToShow: 2
				  	}
			},
			{
				breakpoint: 567,
					settings: {
						slidesToShow: 1
					}
			}]
		});
	}
	
	if($('.indexnine .features-slider').length > 0) {
		$('.indexnine .features-slider').slick({
			dots: true,
			infinite: true,
			centerMode: true,
			slidesToShow: 3,
			speed: 500,
			variableWidth: true,
			arrows: false,
			autoplay:false,
			responsive: [{
				  breakpoint: 992,
				  settings: {
					slidesToShow: 1
				  }

			}]
		});
	}
	
	//Counter
	
	$('.indexnine .counter-count').each(function () {
        $(this).prop('Counter',0).animate({
            Counter: $(this).text()
        }, {
            duration: 5000,
            easing: 'swing',
            step: function (now) {
                $(this).text(Math.ceil(now));
            }
        });
    });	
	
	// Home 10
	
	// Slick Slider
	
	if($('.indexten .top-restaurant-slider').length > 0) {
		$('.top-restaurant-slider').slick({
			dots: false,
			autoplay:false,
			infinite: true,
			variableWidth: true,
			rows: 2
		});
	}

	if($('.indexten .top-food-slider').length > 0) {
		$('.top-food-slider').slick({
			dots: false,
			autoplay:false,
			infinite: true,
			variableWidth: true,
			slideToShow: 4,
			slideToScroll: 1,
			rows: 2
		});
	}

	if($('.indexten .top-chef-slider').length > 0) {
		$('.top-chef-slider').slick({
			dots: false,
			autoplay:false,
			infinite: true,
			variableWidth: true,
			slideToShow: 4,
			slideToScroll: 1,
		});
	}
	
    // Home 11
	
	// Slick Slider
	
	if ($('.indexeleven .sslider').length > 0) {
        $('.indexeleven .sslider').slick({
			dots: false,
			autoplay:false,
			infinite: true,
            variableWidth: false,
            slidesToShow: 4,
            responsive: [
                {
                    breakpoint: 1024,
                    settings: {
                        slidesToShow: 3,
                        slidesToScroll: 3
                    }
                },
                {
                    breakpoint: 767,
                    settings: {
                        slidesToShow: 2,
                        slidesToScroll: 2
                    }
                },
                {
                    breakpoint: 480,
                    settings: {
                        slidesToShow: 1,
                        slidesToScroll: 1
                    }
                }
                ]
		});
    }
    
	if ($('.indexeleven .review-slider').length > 0) {
        $('.indexeleven .review-slider').slick({
			dots: false,
			autoplay:false,
			infinite: true,
            variableWidth: false,
            slidesToShow: 3,
            arrows: false,
            responsive: [
                {
                    breakpoint: 1024,
                    settings: {
                        slidesToShow: 3,
                        slidesToScroll: 3
                    }
                },
                {
                    breakpoint: 767,
                    settings: {
                        slidesToShow: 2,
                        slidesToScroll: 2
                    }
                },
                {
                    breakpoint: 480,
                    settings: {
                        slidesToShow: 1,
                        slidesToScroll: 1
                    }
                }
                ]
		});
    }
	
	// Home 12
	
	//Multi -items slider
	
	if($('.indextwelve .carousel').length > 0) {
		var slickopts = {
		  slidesToShow: 3,
		  slidesToScroll: 1,
		  rows: 1,
		  responsive: [
		    { breakpoint: 992,
		      settings: {
		        slidesToShow: 3
		      }
		    },
		    { breakpoint: 776,
		      settings: {
		        slidesToShow: 2,
		        rows: 1
		      }
		    },
		    { breakpoint: 567,
		      settings: {
		        slidesToShow: 1,
		        rows: 1
		      }
		    }]
		};
		$('.carousel').slick(slickopts);
	}
	
	if($('.indextwelve .specialities-slider').length > 0) {
		$('.indextwelve .specialities-slider').slick({
			dots: false,
			autoplay:false,
			infinite: true,
			slidesToShow: 4,
			slidesToScroll: 1,
			responsive: [{
				breakpoint: 992,
					settings: {
						slidesToShow: 3
				  	}
			},
			{
				breakpoint: 800,
					settings: {
						slidesToShow: 3
				  	}
			},
			{
				breakpoint: 776,
					settings: {
						slidesToShow: 2
				  	}
			},
			{
				breakpoint: 567,
					settings: {
						slidesToShow: 1
					}
			}]
		});
	}
	
	// Home 13
	
	//Sports Slider

    if($('.indexthirteen .owl-carousel.coaches-list').length > 0) { 
	$('.indexthirteen .owl-carousel.coaches-list').owlCarousel({
		loop:true,
		margin:15,
        dots: true,
		responsive:{
			0:{
				items:1
			},
			500:{
				items:2
			},
			768:{
				items:3
			},
			1000:{
				items:4
			},
			1300:{
				items:5
			}
		}
	})
	}
	
	//Coaches Slider
    
    if($('.indexthirteen .owl-carousel.best-coaches').length > 0) { 	
	$('.indexthirteen .owl-carousel.best-coaches').owlCarousel({
		loop:true,
		margin:15,
        dots: true,
		responsive:{
			0:{
				items:1
			},
			500:{
				items:2
			},
			768:{
				items:3
			},
			1000:{
				items:4
			},
			1300:{
				items:5
			}
		}
	})
    }
	
	//Testimonial Slider
	
     if($('.indexthirteen .owl-carousel.testimonials').length > 0) { 	
	$('.indexthirteen .owl-carousel.testimonials').owlCarousel({
		loop:true,
		margin:15,
        dots: true,
		responsive:{
			0:{
				items:1
			},
			500:{
				items:2
			},
			768:{
				items:2
			},
			1000:{
				items:3
			},
			1300:{
				items:3
			}
		}
	})
	 }
	
	//Home 14
	
	//Top Universities Slider

     if($('.indexfourteen .owl-carousel.university').length > 0) { 	
	$('.indexfourteen .owl-carousel.university').owlCarousel({
		loop:true,
		margin:15,
		nav:true,
		navText: ['<span class="fas fa-chevron-left"></span>','<span class="fas fa-chevron-right"></span>'],
		navContainer: '.slide-nav-1',
		responsive:{
			0:{
				items:1
			},
			500:{
				items:1
			},
			768:{
				items:2
			},
			1000:{
				items:3
			},
			1300:{
				items:4
			}
		}
	})
	 }
	
	//Popular Courses Slider

    if($('.indexfourteen .owl-carousel.popular-courses').length > 0) {
	$('.indexfourteen .owl-carousel.popular-courses').owlCarousel({
		loop:true,
		margin:15,
		nav:true,
		navText: ['<span class="fas fa-chevron-left"></span>','<span class="fas fa-chevron-right"></span>'],
		navContainer: '.slide-nav-2',
		responsive:{
			0:{
				items:1
			},
			500:{
				items:1
			},
			768:{
				items:1
			},
			1000:{
				items:2
			},
			1300:{
				items:2
			}
		}
	})
    }
	
	
})(jQuery);



