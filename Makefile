WEBROOT := ./wwwroot
NODEPATH :=./node_modules/.bin

SCRIPTJS :=$(WEBROOT)/js/script.js
SCRIPTMINJS :=$(WEBROOT)/js/script.min.js

STYLELESS :=$(WEBROOT)/css/style.less
STYLECSS :=$(WEBROOT)/css/style.css
STYLEMINCSS :=$(WEBROOT)/css/style.min.css

.PHONY: all clean npm bower bootstrap

all:  $(STYLECSS) $(STYLEMINCSS) $(SCRIPTMINJS)

clean:
	rm -f $(SCRIPTMINJS)
	rm -f $(WEBROOT)/css/style.min.css
	rm -f $(WEBROOT)/css/style.css

npm:
	npm install

bower: 
	$(NODEPATH)/bower install

bootstrap: npm bower all

$(SCRIPTMINJS): $(SCRIPTJS)
	$(NODEPATH)/uglifyjs --compress --mangle -o $(SCRIPTMINJS) -- \
		$(WEBROOT)/bower_components/nanoajax/nanoajax.min.js \
		$(WEBROOT)/bower_components/vue/dist/vue.js \
		$(SCRIPTJS)

$(STYLECSS): $(STYLELESS)
	$(NODEPATH)/lessc $(STYLELESS) $(STYLECSS)

$(STYLEMINCSS): $(STYLECSS)
	$(NODEPATH)/cleancss -o $(STYLEMINCSS) \
		$(WEBROOT)/bower_components/normalize-css/normalize.css \
		$(STYLECSS)