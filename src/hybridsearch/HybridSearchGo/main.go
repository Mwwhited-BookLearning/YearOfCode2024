package main

import (
	"hybrid-search/webapi/controllers/files"
	"net/http"
)

func main() {
	files.AddRoutes()
	http.ListenAndServe("0.0.0.0:3080", nil)
}
