// Função de pré-processamento
function process(data) {
  const corporativeData = data.filter(item => item.produto === "Empresarial");

  const byQuarterAndIssuer = corporativeData.reduce((acc, item) => {
    const key = `${item.trimestre}-${item.nomeBandeira}`;
    if (!acc[key]) {
      acc[key] = {
        trimestre: item.trimestre,
        nomeBandeira: item.nomeBandeira,
        qtdCartoesEmitidos: 0,
        qtdCartoesAtivos: 0,
        qtdTransacoesNacionais: 0,
        valorTransacoesNacionais: 0,
      };
    }
    acc[key].qtdCartoesEmitidos += item.qtdCartoesEmitidos;
    acc[key].qtdCartoesAtivos += item.qtdCartoesAtivos;
    acc[key].qtdTransacoesNacionais += item.qtdTransacoesNacionais;
    acc[key].valorTransacoesNacionais += item.valorTransacoesNacionais;
    return acc;
  }, {});

  return Object.values(byQuarterAndIssuer);
}