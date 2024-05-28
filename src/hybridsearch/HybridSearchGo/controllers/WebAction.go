package controllers

import (
	"github.com/gin-gonic/gin"
)

type WebAction struct {
	Pattern string
	Handler func(*gin.Context)
	Method  string
}
