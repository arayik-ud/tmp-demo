import express from 'express';
import cors from 'cors';
const app = express();
const port = 8080;

app.use(cors());

app.get('/', (_, res) => {
  const payload = {
    text: '<align=left><color=#00b3d6><b>Lorem ipsum dolor sit amet, consectetur adipiscing elit. Mauris nec tempor nibh. Nulla elementum mi vel ex maximus sodales. Phasellus non tortor sit amet orci pulvinar dignissim. Duis pharetra, ligula nec lobortis tincidunt, turpis sem consequat ex, sit amet convallis ligula lectus id nisi. Praesent posuere augue risus, et tincidunt magna pellentesque a. Integer pulvinar, elit ac gravida dignissim, nulla tellus faucibus ipsum, eu mattis arcu quam at nisi.</b></color><color=#00b3d6> </color><color=#000><mark=#ffff00aa>Mauris nec elementum dui. Nullam nec pellentesque nulla. Donec eu laoreet metus.</mark></color><color=#00b3d6> </color><color=#000><i>Nam sed erat eu sapien hendrerit euismod convallis vitae erat.</i></color><color=#00b3d6> </color><color=#000><u>Curabitur vel velit diam. Morbi eleifend lectus et nulla varius, vel interdum neque rutrum.</u></color></align>\n<align=left></align>\n<align=right><color=#000>Praesent quis tortor at risus vehicula vehicula. Curabitur sodales fermentum felis vel blandit. Nunc semper est sit amet ipsum ultricies rutrum in in tellus. Donec consequat fermentum lorem nec tempus. Nam blandit sem nec vestibulum vestibulum.</color></align>\n<align=left><color=#000><style=H1>Heading 1</style></color></align>\n<align=left><color=#000><style=H2>Heading 2</style></color></align>\n<align=left><color=#000> </color></align>\n<align=right><color=#c45a03><i>Donec et nisl et sapien pharetra euismod at eleifend diam. Sed lacus ipsum, vulputate eu nulla eget, malesuada tincidunt felis. Nulla pretium mauris neque, a fermentum magna placerat vitae. Aliquam auctor sapien a ligula pretium scelerisque.</i></color><color=#c45a03> Phasellus vitae sem augue. Pellentesque accumsan suscipit efficitur. Curabitur nec dolor vel purus scelerisque gravida non ut diam. Vestibulum ante ipsum primis in faucibus orci luctus et ultrices posuere cubilia curae;</color></align>\n<size=400><sprite index=0> <size=500><sprite index=1>',
    images: [
      'https://cdn.pixabay.com/photo/2015/12/01/20/28/road-1072821__340.jpg',
      'https://thumbs.dreamstime.com/b/environment-earth-day-hands-trees-growing-seedlings-bokeh-green-background-female-hand-holding-tree-nature-field-gra-130247647.jpg',
    ],
  };
  res.send(payload);
});

app.listen(port, () => {
  console.log(`Server listening to ${port}`);
});
