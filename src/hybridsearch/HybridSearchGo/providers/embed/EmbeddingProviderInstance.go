package embed

import (
	"encoding/json"
	"fmt"
	"hybrid-search/webapi/providers"
	"io"
	"log"
	"net/http"
)

type EmbeddingProviderInstance struct {
	Lexical  providers.LexicalSearchProvider
	Semantic providers.SemanticSearchProvider
}

func Create() EmbeddingProviderInstance {
	return EmbeddingProviderInstance{}
}

type ResponseData struct {
	Embedding []float32 `json:"embedding"`
	// Add other fields as needed based on the structure of your JSON response
}

func (provider EmbeddingProviderInstance) Embed(text string) ([]float32, error) {
	response, err := http.Get(fmt.Sprintf("http://192.168.1.170:5080/generate-embedding?query=%s", text))

	if err != nil {
		log.Println("Error:", err)
		return nil, err
	}

	defer response.Body.Close()
	body, err := io.ReadAll(response.Body)
	if err != nil {
		log.Println("Error:", err)
		return nil, err
	}

	var responseData ResponseData
	if err := json.Unmarshal(body, &responseData); err != nil {
		log.Println("Error parsing response body as JSON:", err)
		return nil, err
	}

	log.Println("Array of floats:", responseData.Embedding)
	return responseData.Embedding, nil
}
