package main

import (
	"hybrid-search/webapi/controllers/files"
	"net/http"
)

func main() {

	routes := files.CreateRoutes()
	files.AddRoutes(routes)

	http.ListenAndServe("0.0.0.0:3080", nil)
}
