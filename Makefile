WEBROOT=./wwwroot
NODEPATH=./node_modules/.bin

.PHONY: all
all:
	make clean
	make less
	make min

.PHONY: clean
clean:
	rm -f $(WEBROOT)/js/script.min.js
	rm -f $(WEBROOT)/css/style.min.css
	rm -f $(WEBROOT)/css/style.css

.PHONY: min
min:
	$(NODEPATH)/uglifyjs --compress --mangle -o $(WEBROOT)/js/script.min.js -- \
		$(WEBROOT)/bower_components/nanoajax/nanoajax.min.js \
		$(WEBROOT)/bower_components/vue/dist/vue.js \
		$(WEBROOT)/js/script.js
	$(NODEPATH)/cleancss -o $(WEBROOT)/css/style.min.css \
		$(WEBROOT)/bower_components/normalize-css/normalize.css \
		$(WEBROOT)/css/style.css

.PHONY: less
less:
	$(NODEPATH)/lessc $(WEBROOT)/css/style.less $(WEBROOT)/css/style.css

.PHONY: bootstrap
bootstrap:
	npm install
	$(NODEPATH)/bower install
	make all