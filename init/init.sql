USE [master]
GO

IF DB_ID('PatteDoie') IS NOT NULL
  set noexec on 

CREATE DATABASE [PatteDoie];
GO