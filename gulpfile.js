
var gulp = require('gulp'),
  rimraf = require('rimraf'),
  concat = require('gulp-concat'),
  cssmin = require('gulp-cssmin'),
  uglify = require('gulp-uglify'),
  less = require('gulp-less');

var paths = {
  webroot: './wwwroot/'
};

paths.minJs = paths.webroot + '/js/script.min.js';
paths.minCss = paths.webroot + '/css/style.min.css';
paths.compiledCss = paths.webroot + '/css/style.css';
paths.js = [paths.webroot + 'bower_components/nanoajax/nanoajax.min.js', paths.webroot + 'bower_components/vue/dist/vue.js', paths.webroot + 'js/script.js'];
paths.css = [paths.webroot + 'bower_components/normalize-css/normalize.css', paths.webroot + 'css/style.css']
paths.less = paths.webroot + 'css/style.less';

gulp.task('clean:js', cb => {
  rimraf(paths.minJs, cb);
});

gulp.task('clean:cssmin', cb => {
  rimraf(paths.minCss, cb);
});

gulp.task('clean:cssCompile', cb => {
    rimraf(paths.compiledCss, cb);
});

gulp.task('less', () => {
    return gulp.src(paths.less)
        .pipe(less())
        .pipe(gulp.dest(paths.webroot + 'css'));
});

gulp.task('clean:css', ['clean:cssCompile', 'clean:cssmin']);

gulp.task('clean', ['clean:js', 'clean:css']);

gulp.task('min:js', () => {
    return gulp.src(paths.js, {base: '.'})
        .pipe(concat(paths.minJs))
        .pipe(uglify())
        .pipe(gulp.dest('.'));
});

gulp.task('min:css', ['less'], () => {
    return gulp.src(paths.css)
        .pipe(concat(paths.minCss))
        .pipe(cssmin())
        .pipe(gulp.dest('.'));
});

gulp.task('min', ['min:js', 'min:css']);

gulp.task('default', ['clean', 'min']);