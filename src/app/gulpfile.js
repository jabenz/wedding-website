'use strict';

var gulp = require('gulp');
var sass = require('gulp-sass')(require('sass'));
var uglify = require('gulp-uglify');
var rename = require('gulp-rename');
var replace = require('gulp-replace');

// Define configuration for different environments
var config = {
    development: {
        inviteCodeMd5: 'e10adc3949ba59abbe56e057f20f883e', // Test Invite Code 123456
        functionApiUrl: 'http://localhost:7234/api'
    },
    production: {
        inviteCodeMd5: 'd2c5d7b92192270fc10ea028eb2a38e8',
        functionApiUrl: 'https://laura-und.marvin-stue.de/api'
    }
};

var env = process.env.NODE_ENV || 'development';
var envConfig = config[env];

// compile scss to css
gulp.task('sass', function () {
    return gulp.src('./sass/styles.scss')
        .pipe(sass({outputStyle: 'compressed'}).on('error', sass.logError))
        .pipe(rename({basename: 'styles.min'}))
        .pipe(gulp.dest('./css'));
});

// watch changes in scss files and run sass task
gulp.task('sass:watch', function () {
    gulp.watch('./sass/**/*.scss', ['sass']);
});

// minify js
gulp.task('minify-js', function () {
    return gulp.src('./js/scripts.js')
        .pipe(replace('__FUNCTION_API_URL__', envConfig.functionApiUrl))
        .pipe(replace('__INVITE_CODE_MD5__', envConfig.inviteCodeMd5))
        .pipe(uglify())
        .pipe(rename({basename: 'scripts.min'}))
        .pipe(gulp.dest('./js'));
});

// default task
gulp.task('default', gulp.series('sass', 'minify-js'));