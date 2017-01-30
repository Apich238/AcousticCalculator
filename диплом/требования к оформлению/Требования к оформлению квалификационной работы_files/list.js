var jq = jQuery.noConflict();

jq(document).ready(function() {
	jq('div#groups li > div').click(function() {
		jq(this).parent().find('>ul, >ol').slideToggle('normal');
	}),
	jq('div#groups div a.expand').click(function() {
		jq('div#groups ul ul, div#groups ul ol').fadeIn('normal');
	}),
	jq('div#groups div a.collapse').click(function() {
		jq('div#groups ul ul, div#groups ul ol').fadeOut('normal');
	}),
	jq('div#groups ul ul, div#groups ul ol').hide();
	
});
