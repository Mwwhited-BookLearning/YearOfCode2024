package main

import (
	"encoding/json"
	"github.com/gorilla/mux"
	"hybrid-search/webapi/controllers/fileController"
	"log"
	"net/http"
)

func main() {
	router := mux.NewRouter()
	router.HandleFunc("/", HomeHandler)

	router.HandleFunc("/api/health", func(w http.ResponseWriter, r *http.Request) {
		// an example API handler
		json.NewEncoder(w).Encode(map[string]bool{"ok": true})
	})

	fileController.AddRoutes(router)

	http.Handle("/", router)
	http.ListenAndServe("0.0.0.0:3080", nil)
}

func HomeHandler(w http.ResponseWriter, r *http.Request) {
	w.WriteHeader(http.StatusOK)
	log.Println("Home:")
}
