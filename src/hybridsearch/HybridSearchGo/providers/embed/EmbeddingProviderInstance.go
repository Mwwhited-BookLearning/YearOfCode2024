package hybrid

import (
	"hybrid-search/webapi/providers"
)

type EmbeddingProviderInstance struct {
	Lexical  providers.LexicalSearchProvider
	Semantic providers.SemanticSearchProvider
}

func Create() EmbeddingProviderInstance {
	return EmbeddingProviderInstance{}
}

func (provider EmbeddingProviderInstance) Embed(text string) []float32 {
	return nil //TODO: complete this
}
