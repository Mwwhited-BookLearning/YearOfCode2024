package lexical

import (
	"crypto/tls"
	opensearch "github.com/opensearch-project/opensearch-go"
	"net/http"
)

type OpenSearchClientFactoryInstance struct {
	Options OpenSearchOptions
}

func CreateClientFactory(options OpenSearchOptions) OpenSearchClientFactory {
	return OpenSearchClientFactoryInstance{
		Options: options,
	}
}

func (factory OpenSearchClientFactoryInstance) Create() *opensearch.Client {

	client, _ := opensearch.NewClient(opensearch.Config{
		Transport: &http.Transport{
			TLSClientConfig: &tls.Config{InsecureSkipVerify: true},
		},
		Addresses: []string{factory.Options.Url()},
	})
	return client
}

// https://opensearch.org/docs/latest/clients/go/
