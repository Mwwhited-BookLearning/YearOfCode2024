package controllers

import (
	"net/http"
)

type WebAction struct {
	Pattern   string
	Handler   func(http.ResponseWriter, *http.Request)
	Method    string
	RouteType RouteTypes
}
