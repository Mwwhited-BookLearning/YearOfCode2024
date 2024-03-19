package main

import (
	//"encoding/json"
	"github.com/gorilla/mux"
	"hybrid-search/webapi/controllers/fileController"
	//"log"
	"net/http"
)

func main() {
	router := mux.NewRouter()
	// router.HandleFunc("/", HomeHandler)

	// router.HandleFunc("/api/health", func(w http.ResponseWriter, r *http.Request) {
	// 	// an example API handler
	// 	json.NewEncoder(w).Encode(map[string]bool{"ok": true})
	// })

	fileController.AddRoutes(router)

	http.Handle("/", router)
	http.ListenAndServe("0.0.0.0:3080", nil)
}

// func HomeHandler(w http.ResponseWriter, r *http.Request) {
// 	w.WriteHeader(http.StatusOK)
// 	log.Println("Home:")
// }


func NewOpenAPI3() openapi3.Swagger {
	swagger := openapi3.Swagger{
		OpenAPI: "3.0.0",
		Info: &openapi3.Info{
			Title:       "ToDo API",
			Description: "REST APIs used for interacting with the ToDo Service",
			Version:     "0.0.0",
			License: &openapi3.License{
				Name: "MIT",
				URL:  "https://opensource.org/licenses/MIT",
			},
			Contact: &openapi3.Contact{
				URL: "https://github.com/MarioCarrion/todo-api-microservice-example",
			},
		},
		Servers: openapi3.Servers{
			&openapi3.Server{
				Description: "Local development",
				URL:         "http://0.0.0.0:9234",
			},
		},
	}