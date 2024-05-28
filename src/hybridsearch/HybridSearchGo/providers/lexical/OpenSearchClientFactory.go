package lexical

import (
	opensearch "github.com/opensearch-project/opensearch-go"
)

type OpenSearchClientFactory interface {
	Create() *opensearch.Client
}
