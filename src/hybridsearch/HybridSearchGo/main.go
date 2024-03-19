package main

import (
	"github.com/gorilla/mux"
	"hybrid-search/webapi/controllers/fileController"
	"net/http"
)

func main() {
	router := mux.NewRouter()

	fileController.AddRoutes(router)

	http.Handle("/", router)
	http.ListenAndServe("0.0.0.0:3080", nil)
}
