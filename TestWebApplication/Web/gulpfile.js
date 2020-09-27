/// <binding AfterBuild='default' />
// Ref :
// https://www.davepaquette.com/archive/2014/10/08/how-to-use-gulp-in-visual-studio.aspx
// https://www.toptal.com/javascript/optimize-js-and-css-with-gulp

// include plug-ins
var gulp = require('gulp');
var concat = require('gulp-concat');
var uglify = require('gulp-uglify');
var del = require('del');
var sass = require('gulp-sass');
var minify = require('gulp-minify');
var minifyCSS = require('gulp-minify-css');
var ts = require('gulp-typescript');
var tsProject = ts.createProject('tsconfig.json');

var config = {
    //Include all js/css files but exclude any min.js/min.css files
    scriptSrc: ['Scripts/**/*.js', '!Scripts/**/*.min.js'],
    cssSrc: ['Styles/**/*.scss', '!Styles/**/*.min.css']
};

// Delete the output file(s)
gulp.task('clean-scripts', function () {
    // del is an async function and not a gulp plugin (just standard nodejs)
    // It returns a promise, so make sure you return that from this task function
    // so gulp knows when the delete is complete
    return del(['wwwroot/js/**.js', 'wwwroot/js/**.min.js']);
});

// Delete the output file(s)
gulp.task('clean-styles', function () {
    // del is an async function and not a gulp plugin (just standard nodejs)
    // It returns a promise, so make sure you return that from this task function
    // so gulp knows when the delete is complete
    return del(['wwwroot/css/**.css', 'wwwroot/css/**.min.css']);
});

// Combine and minify all files from the wwwroot folder
// This tasks depends on the clean task which means gulp will ensure that the 
// Clean task is completed before running the scripts task.
gulp.task('scripts', gulp.parallel('clean-scripts', function () {
    // This code will compile and then minify the ts to js
    // https://www.typescriptlang.org/docs/handbook/gulp.html
    return tsProject.src()
        .pipe(tsProject())
        .js.pipe(gulp.dest('wwwroot/js/'));

    // The commented code is for pure js. 
    //return gulp.src(config.scriptSrc)
    //    .pipe(uglify())
    //    //.pipe(concat('site.js'))
    //    .pipe(minify())
    //    .pipe(gulp.dest('wwwroot/js/'));
}));

// Combine and minify all files from the wwwroot folder
// This tasks depends on the clean task which means gulp will ensure that the 
// Clean task is completed before running the scripts task.
gulp.task('styles', gulp.parallel('clean-styles', function () {
    return gulp.src(config.cssSrc)
        .pipe(sass({ outputStyle: 'compressed' }))
        //.pipe(concat('site.css')) // This is commented, because then it will concat all in to a one file
        .pipe(minifyCSS())
        .pipe(gulp.dest('wwwroot/css/'));
}));

// Set a default tasks
gulp.task('default', gulp.parallel(gulp.series('scripts', 'styles'), function () { }));