package embed

import (
	"encoding/json"
	"fmt"
	"io"
	"log"
	"net/http"
)

type EmbeddingProviderInstance struct {
	Options SBertOptions
}

func Create(options SBertOptions) EmbeddingProviderInstance {
	return EmbeddingProviderInstance{
		Options: options,
	}
}

func (provider EmbeddingProviderInstance) Embed(text string) ([]float32, error) {
	response, err := http.Get(fmt.Sprintf("%s/generate-embedding?query=%s", provider.Options.Url, text))

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

func (provider EmbeddingProviderInstance) Length() int {
	ret, _ := provider.Embed("hello world!")
	return len(ret)
}
