@echo off
del /s /a *.meta

call CBAO "obj" %cd%
call CBAO "bin" %cd%